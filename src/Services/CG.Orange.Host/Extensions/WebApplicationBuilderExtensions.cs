
using CG.Orange.Host.Options;
using CG.Orange.SqlLite.Options;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
internal static class WebApplicationBuilderExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method registers the identity services required to integrate 
    /// with an ODIC identity authority.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to 
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section name to use
    /// for the operation.</param>
    /// <param name="bootstrapLogger">The optional bootstrap logger to use 
    /// for the operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style</returns>
    public static WebApplicationBuilder AddOrangeIdentity(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "Identity",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring Identity options from the {section} section",
            sectionName
            );

        // Configure the identity options.
        webApplicationBuilder.Services.ConfigureOptions<IdentityOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var identityOptions
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Adding authentication and authorization services."
            );

        // Wire up the authentication and authorization services.
        webApplicationBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
        }).AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            // Where our identity server is.
            options.Authority = identityOptions.Authority; 

            // Are we in a development environment?
            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                // Don't require HTTPS for meta-data
                options.RequireHttpsMetadata = false;
            }

            // This is who we are.
            options.ClientId = identityOptions.ClientId; 

            // This is what we know.
            options.ClientSecret = identityOptions.ClientSecret;

            // We want an authentication code response.
            options.ResponseType = "code";
            
            // Map the name claim so ASP.NET will understand it.
            options.MapInboundClaims = false;

            // Access and Refresh token stored in the authentication properties.
            options.SaveTokens = true;

            // Go to the user info endpoint for additional claims.
            options.GetClaimsFromUserInfoEndpoint = true;

            // We want these scopes, by default.
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");

            // Map role claim(s) so ASP.NET will understand them.
            options.ClaimActions.MapJsonKey("role", "role", "role");

            // Require these types for a valid token.
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            // Tap into the ODIC events.
            options.Events = new OpenIdConnectEvents
            {
                // On access denied, we want to go back to our
                //   own home page.
                OnAccessDenied = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                }
            };
        });

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
