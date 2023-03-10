
namespace CG.Orange.Plugins.Caching;

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
            "Configuring in-memory cache plugin from the {section} section",
            $"{(configuration as IConfigurationSection)?.Path}:Options"
            );

        // Configure the processor options.
        webApplicationBuilder.Services.ConfigureOptions<ProcessorOptions>(
            configuration.GetSection("Options"),
            out var inMemoryCacheProcessorOptions
            );

        // Log what we're about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the in-memory cache processor"
            );

        // Wire up the in-memory cache processor.
        webApplicationBuilder.Services.AddScoped<ICacheProcessor, CacheProcessor>();
        webApplicationBuilder.Services.AddScoped<CacheProcessor>();

        // Log what we're about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the in-memory distributed cache"
            );

        // NOTE: this could become an issue if the host ever registers
        //   another type of distributed cache.  

        // Wire up the in-memory cache service.
        webApplicationBuilder.Services.AddDistributedMemoryCache();
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
            "Adding middleware for the in-memory cache processor"
            );

        // TODO : add your plugin's startup / pipeline logic here.
    }

    #endregion
}
