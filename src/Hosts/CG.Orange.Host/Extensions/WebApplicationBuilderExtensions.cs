
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
internal static class WebApplicationBuilderExtensions005
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

        // Configure the hosted services part of the BLL _settingFileCountChartOptions.
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

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Adding SignalR services."
            );

        // Add SignalR
        webApplicationBuilder.Services.AddSignalR(options =>
        {
            // Is this a development environment?
            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                // Enable better errors.
                options.EnableDetailedErrors = true;
            }
        });

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Adding the SignalR hubs."
            );

        // Add our SignalR hub.
        webApplicationBuilder.Services.AddSingleton<SignalRHub>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}

