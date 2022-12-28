
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
        webApplicationBuilder.Services.ConfigureOptions<OrangeIdentityOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var identityOptions
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Clearing default inbound claims mapping."
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
                // On access denied, we want to go back to our
                //   own home page.
                OnAccessDenied = context =>
                {
                    bootstrapLogger?.LogInformation("OnAccessDenied");
                    context.HandleResponse();
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                },
                OnSignedOutCallbackRedirect = context =>
                {
                    bootstrapLogger?.LogInformation("OnSignedOutCallbackRedirect");
                    return Task.CompletedTask;
                },
                OnUserInformationReceived = context =>
                {
                    bootstrapLogger?.LogInformation("OnUserInformationReceived");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    bootstrapLogger?.LogInformation("OnTokenValidated");
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    bootstrapLogger?.LogInformation("OnAuthenticationFailed");
                    return Task.CompletedTask;
                },
                OnAuthorizationCodeReceived = context =>
                {
                    bootstrapLogger?.LogInformation("OnAuthorizationCodeReceived");
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    bootstrapLogger?.LogInformation("OnMessageReceived");
                    return Task.CompletedTask;
                },
                OnRedirectToIdentityProvider = context =>
                {
                    bootstrapLogger?.LogInformation("OnRedirectToIdentityProvider");
                    return Task.CompletedTask;
                },
                OnRedirectToIdentityProviderForSignOut = context =>
                {
                    bootstrapLogger?.LogInformation("OnRedirectToIdentityProviderForSignOut");
                    return Task.CompletedTask;
                },
                OnRemoteFailure = context =>
                {
                    bootstrapLogger?.LogInformation("OnRemoteFailure");
                    return Task.CompletedTask;
                },
                OnRemoteSignOut = context =>
                {
                    bootstrapLogger?.LogInformation("OnRemoteSignOut");
                    return Task.CompletedTask;
                },
                OnTicketReceived = context =>
                {
                    bootstrapLogger?.LogInformation("OnTicketReceived");
                    return Task.CompletedTask;
                },
                OnTokenResponseReceived = context =>
                {
                    bootstrapLogger?.LogInformation("OnTokenResponseReceived");
                    return Task.CompletedTask;
                },
            };
        });

        // Add the token cache.
        webApplicationBuilder.Services.AddScoped<TokenCache>();

        // Add the HTTP client.
        webApplicationBuilder.Services.AddHttpClient();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
