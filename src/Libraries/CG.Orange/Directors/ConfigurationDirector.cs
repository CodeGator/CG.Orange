
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IConfigurationDirector"/>
/// interface.
/// </summary>
internal class ConfigurationDirector : IConfigurationDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the processor factory for this director.
    /// </summary>
    internal protected readonly IProcessorFactory _processorFactory;

    /// <summary>
    /// This field contains the setting file manager for this director.
    /// </summary>
    internal protected readonly ISettingFileManager _settingFileManager = null!;

    /// <summary>
    /// This field contains the provider manager for this director.
    /// </summary>
    internal protected readonly IProviderManager _providerManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IConfigurationDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ConfigurationDirector"/>
    /// class.
    /// </summary>
    /// <param name="processorFactory">The processor factory to use with
    /// this director.</param>
    /// <param name="settingFileManager">The setting file manager to use 
    /// with this director.</param>
    /// <param name="providerManager">The provider manager to use with
    /// this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public ConfigurationDirector(
        IProcessorFactory processorFactory,
        ISettingFileManager settingFileManager,
        IProviderManager providerManager,
        ILogger<IConfigurationDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processorFactory, nameof(processorFactory))
            .ThrowIfNull(settingFileManager, nameof(settingFileManager))
            .ThrowIfNull(providerManager, nameof(providerManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _processorFactory = processorFactory;
        _settingFileManager = settingFileManager;
        _providerManager = providerManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<Dictionary<string, string>> ReadConfigurationAsync(
        string applicationName, 
        string? environmentName, 
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(applicationName, nameof(applicationName));

        // Here we're building a complete configuration for the given
        //   application and/or environment - with associated secret
        //   values.

        try
        {
            // Step 1: get the configuration settings for the application and environment.

            // Was an environment specified?
            SettingFileModel? envFile = null;
            if (!string.IsNullOrEmpty(environmentName))
            {
                // Look for the environment file.
                envFile = await _settingFileManager.FindByApplicationAndEnvironmentAsync(
                    applicationName,
                    environmentName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Step 2: convert the JSON to a configuration.

            // Do we have an environment file?
            IConfiguration? envConfiguration = null;
            if (envFile is not null)
            {
                // Wrap the JSON for the configuration builder.
                using var stream = new MemoryStream(
                    Encoding.UTF8.GetBytes(envFile.Json)
                    );

                // Read the JSON as a configuration.
                envConfiguration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
            }

            // Step 3: get the base configuration settings for the application.

            // Look for the base file (no environment).
            var baseFile = await _settingFileManager.FindByApplicationAndEnvironmentAsync(
                applicationName,
                "",
                cancellationToken
                ).ConfigureAwait(false);

            // Step 4: convert the JSON to a configuration.

            // Do we have a base file?
            IConfiguration? baseConfiguration = null;
            if (baseFile is not null)
            {
                // Wrap the JSON for the configuration builder.
                using var stream = new MemoryStream(
                    Encoding.UTF8.GetBytes(baseFile.Json)
                    );

                // Read the JSON as a configuration.
                baseConfiguration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
            }

            // Step 5: merge the configurations together.

            var builder = new ConfigurationBuilder();

            // Do we have a base configuration?
            if (baseConfiguration is not null)
            {
                // Add in the settings.
                builder.AddInMemoryCollection(
                        baseConfiguration.AsEnumerable().Select(x =>
                            new KeyValuePair<string, string?>(x.Key, x.Value)
                            )
                        );
            }

            // Do we have an environment configuration?
            if (envConfiguration is not null)
            {
                // Add in the settings.
                builder.AddInMemoryCollection(
                        envConfiguration.AsEnumerable().Select(x =>
                            new KeyValuePair<string, string?>(x.Key, x.Value)
                            )
                        );
            }

            // Merge the configuration(s).
            var mergedConfiguration = builder.Build();

            // Step 6: replace any tokens.
            var kvpCollection = await ReplaceTokensAsync(
                applicationName,
                environmentName,
                mergedConfiguration,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return kvpCollection;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a configuration!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The manager failed to search for a configuration!",
                innerException: ex
                );
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    private async Task<Dictionary<string, string>> ReplaceTokensAsync(
        string applicationName,
        string? environmentName,
        IConfigurationRoot source,
        CancellationToken cancellationToken = default
        )
    {
        var dest = new Dictionary<string, string>();

        // Loop through the settings.
        foreach (var setting in source.AsEnumerable())
        {
            // Get the key and value.
            var key = setting.Key;
            var value = setting.Value ?? "";  
            
            try
            {
                var secretTag = "";
                var cacheTag = "";

                // Sanity check the value.
                if (!string.IsNullOrEmpty(value))
                {
                    // Look for a replacement token.
                    if (value.StartsWith("##") && value.EndsWith("##"))
                    {
                        // Trim the token delimiters.
                        value = value.TrimStart('#').TrimEnd('#');
                        
                        // Try to split the token.
                        var parts = value.Split(':');

                        // Save the secret tag.
                        secretTag = parts[0];

                        // Is there a cache tag?
                        if (parts.Length > 1)
                        {
                            // Save the cache tag.
                            cacheTag = parts[1];    
                        }

                        // We'll supply this from a secret, or the cache, later.
                        value = "";
                    }

                    // Is there a cache tag?
                    ProviderModel? cacheProvider = null;
                    if (!string.IsNullOrEmpty(cacheTag))
                    {
                        // Look for a matching provider.
                        cacheProvider = await _providerManager.FindByTagAndTypeAsync(
                            cacheTag,
                            ProviderType.Cache,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Did we fail?
                        if (cacheProvider is null)
                        {
                            // Panic!!
                            throw new KeyNotFoundException(
                                $"The cache provider tag: {cacheTag} wasn't found!"
                                );
                        }
                    }

                    // Is there a cache provider?
                    ICacheProcessor? cacheProcessor = null;
                    if (cacheProvider is not null)
                    {
                        // Get the cache processor.
                        cacheProcessor = await _processorFactory.CreateCacheProcessorAsync(
                            cacheProvider,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Did we succeed?
                        if (cacheProcessor is not null)
                        {
                            // Try to find the value.
                            value = await cacheProcessor.GetValueAsync(
                                cacheProvider,
                                key,
                                cancellationToken
                                ).ConfigureAwait(false);
                        }
                    }

                    // Do we still need the secret value?
                    if (string.IsNullOrEmpty(value))
                    {
                        // Look for a matching provider.
                        var secretProvider = await _providerManager.FindByTagAndTypeAsync(
                            secretTag,
                            ProviderType.Secret,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Did we fail?
                        if (cacheProvider is null)
                        {
                            // Panic!!
                            throw new KeyNotFoundException(
                                $"The secret provider tag: {secretTag} wasn't found!"
                                );
                        }

                        // Is there a secret provider?
                        if (secretProvider is not null)
                        {
                            // Get the secret processor.
                            var secretProcessor = await _processorFactory.CreateSecretProcessorAsync(
                                secretProvider,
                                cancellationToken
                                ).ConfigureAwait(false);

                            // Did we succeed?
                            if (secretProcessor is not null)
                            {
                                // Try to find the value.
                                value = await secretProcessor.GetValueAsync(
                                    secretProvider,
                                    key,
                                    cancellationToken
                                    ).ConfigureAwait(false);
                            }
                        }

                        // Is there a cache provider?
                        if (cacheProvider is not null)
                        {
                            // Is there a cache processor?
                            if (cacheProcessor is not null)
                            {
                                // Try to cache the value.
                                await cacheProcessor.SetValueAsync(
                                    cacheProvider,
                                    key,
                                    value ?? "",
                                    cancellationToken
                                    ).ConfigureAwait(false);
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                // Log what happened.
                _logger.LogError(
                    ex,
                    "Failed to fetch a secret for application {app}, " +
                    "environment: {env}, and key: {key}",
                    applicationName,
                    environmentName,
                    key
                    );

                // Replace the token, since we failed to fetch the secret.
                value = setting.Value ?? "";
            }
            
            // Add the setting.
            dest.Add(key, value ?? "");
        }

        // Return the results.
        return dest;
    }

    #endregion
}
