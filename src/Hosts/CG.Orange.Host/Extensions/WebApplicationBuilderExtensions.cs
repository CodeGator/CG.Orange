
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
    /// This method registers local services that are required for the host
    /// to run properly.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to 
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section name to use
    /// for the operation.</param>
    /// <param name="bootstrapLogger">The optional bootstrap logger to use 
    /// for the operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style</returns>
    public static WebApplicationBuilder AddOrangeServices(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "BLL",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Build the complete section name.
        var completeSectionName = $"{sectionName}:HostedServices";

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring hosted services options from the {section} section",
            completeSectionName
            );

        // Configure the hosted services part of the BLL options.
        webApplicationBuilder.Services.ConfigureOptions<HostedServicesOptions>(
            webApplicationBuilder.Configuration.GetSection(completeSectionName),
            out var hostedServiceOptions
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Adding an HTTP client."
            );

        // Add the HTTP client.
        webApplicationBuilder.Services.AddHttpClient();

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Adding hosted services."
            );

        // Add the hosted services.
        webApplicationBuilder.Services.AddHostedService<WarmupService>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    // *******************************************************************

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
            "Configuring identity options from the {section} section",
            sectionName
            );

        // Configure the identity options.
        webApplicationBuilder.Services.ConfigureOptions<OrangeIdentityOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var identityOptions
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Clearing default inbound claims mapping for identity."
            );

        // Clear default inbound claim mapping.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
            options.Cookie.Name = identityOptions.CookieName;
            options.Cookie.SameSite = SameSiteMode.Strict;
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

            options.ResponseMode = "query";

            // Don't map claims.
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
                // On access denied, we want to go back to our home page.
                OnAccessDenied = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                }
            };
        }).AddJwtBearer(options =>
        {
            options.Authority = identityOptions.Authority;
            options.TokenValidationParameters.ValidateAudience = false;
        }); 
        
        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
