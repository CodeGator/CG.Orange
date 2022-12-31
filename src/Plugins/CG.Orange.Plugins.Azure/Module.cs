
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CG.Orange.Plugins.Azure.Options;

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
            "Configuring Azure provider from the {section} section",
            $"{(configuration as IConfigurationSection)?.Path}:Options"
            ); 

        // Configure the provider options.
        webApplicationBuilder.Services.ConfigureOptions<AzureProviderOptions>(
            configuration.GetSection("Options"),
            out var azureProviderOptions
            );

        // Log what we're about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Azure secret client"
            );

        // Wire up the Azure secret client.
        webApplicationBuilder.Services.AddScoped<SecretClient>(serviceProvider =>
        {
            // Create retry options, in case the key vault is throttled.
            var options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(9),
                    MaxRetries = 3,
                    Mode = RetryMode.Exponential
                }
            };

            // Create the Azure client.
            var client = new SecretClient(
                new Uri(azureProviderOptions.KeyVaultUri),
                new DefaultAzureCredential(),
                options
                );

            // Return the results.
            return client;
        });

        // Log what we're about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Azure providers"
            );

        // Wire up the Azure provider.
        webApplicationBuilder.Services.AddScoped<ISecretProvider, AzureSecretProvider>();
    }

    // *******************************************************************

    /// <inheritdoc/>
    public override void Configure(
        WebApplication webApplication
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplication, nameof(webApplication));

        // TODO : add your plugin's startup / pipeline logic here.
    }

    #endregion
}
