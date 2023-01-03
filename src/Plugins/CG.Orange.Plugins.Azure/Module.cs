
namespace CG.Orange.Plugins.Azure;

/// <summary>
/// This class represents the plugin module's startup logic.
/// </summary>
public class Module : ModuleBase
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public override void ConfigureServices(
        WebApplicationBuilder webApplicationBuilder,
        IConfiguration configuration,
        ILogger? bootstrapLogger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNull(configuration, nameof(configuration));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring Azure plugin from the {section} section",
            $"{(configuration as IConfigurationSection)?.Path}:Options"
            ); 

        // Configure the processor options.
        webApplicationBuilder.Services.ConfigureOptions<AzureSecretProcessorOptions>(
            configuration.GetSection("Options"),
            out var azureSecretProcessorOptions
            );

        // Log what we're about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Azure secret client factory"
            );

        // Wire up the Azure secret client factory.
        webApplicationBuilder.Services.AddScoped<AzureSecretClientFactory>();

        // Log what we're about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Azure secret processor"
            );

        // Wire up the Azure secret processor.
        webApplicationBuilder.Services.AddScoped<ISecretProcessor, AzureSecretProcessor>();
        webApplicationBuilder.Services.AddScoped<AzureSecretProcessor>();
    }

    // *******************************************************************

    /// <inheritdoc/>
    public override void Configure(
        WebApplication webApplication
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplication, nameof(webApplication));

        // Log what we're about to do.
        webApplication.Logger.LogDebug(
            "Adding middleware for the Azure secret processor"
            );

        // TODO : add your plugin's startup / pipeline logic here.
    }

    #endregion
}
