
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

        // Unmap incoming claims, which are mapped, by default, by ASP.NET.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
            options.Authority = "https://localhost:7129";
            options.RequireHttpsMetadata = false;

            options.ClientId = "cg.orange.host";
            options.ClientSecret = "secret";

            options.ResponseType = "code";

            options.MapInboundClaims = false;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

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
                OnAccessDenied = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                },
                // Uncomment to peek at the token.
                //OnTokenValidated = context =>
                //{
                //    var token = context.SecurityToken.RawData;
                //    return Task.CompletedTask;
                //}
            };
        });

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
