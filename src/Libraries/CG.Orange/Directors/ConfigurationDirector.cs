
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
        //   values. This is the only place we'll ever combine the
        //   settings with their associated secrets.

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for an environment argument"
                );

            // Was an environment specified?
            SettingFileModel? envFile = null;
            if (!string.IsNullOrEmpty(environmentName))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Checking for setting file for application: {app}, environment: {env}",
                    applicationName,
                    environmentName
                    );

                // Look for the environment file.
                envFile = await _settingFileManager.FindByApplicationAndEnvironmentAsync(
                    applicationName,
                    environmentName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for an environment setting file"
                );

            // Do we have an environment file?
            IConfiguration? envConfiguration = null;
            if (envFile is not null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Reading JSON into a stream"
                    );

                // Wrap the JSON for the configuration builder.
                using var stream = new MemoryStream(
                    Encoding.UTF8.GetBytes(envFile.Json)
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Reading JSON into a configuration builder"
                    );

                // Read the JSON as a configuration.
                envConfiguration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Reading base setting file"
                );

            // Look for the base file (no environment).
            var baseFile = await _settingFileManager.FindByApplicationAndEnvironmentAsync(
                applicationName,
                "",
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for a base setting file"
                );

            // Do we have a base file?
            IConfiguration? baseConfiguration = null;
            if (baseFile is not null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Reading JSON into a stream"
                    );

                // Wrap the JSON for the configuration builder.
                using var stream = new MemoryStream(
                    Encoding.UTF8.GetBytes(baseFile.Json)
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Reading JSON into a configuration builder"
                    );

                // Read the JSON as a configuration.
                baseConfiguration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a unified configuration"
                );

            var builder = new ConfigurationBuilder();

            // Do we have a base configuration?
            if (baseConfiguration is not null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Merging base configurations"
                    );

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
                // Log what we are about to do.
                _logger.LogDebug(
                    "Merging environment configurations"
                    );

                // Add in the settings.
                builder.AddInMemoryCollection(
                        envConfiguration.AsEnumerable().Select(x =>
                            new KeyValuePair<string, string?>(x.Key, x.Value)
                            )
                        );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Building the merged configuration"
                );

            // Merge the configuration(s).
            var mergedConfiguration = builder.Build();

            // Log what we are about to do.
            _logger.LogDebug(
                "Replacing tokens with secrets"
                );

            // Replace any tokens with secrets.
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

    /// <summary>
    /// This method scans through the given configuration replacing tokens
    /// with their associated secrets.
    /// </summary>
    /// <param name="applicationName">The name of the application to use 
    /// for the operation.</param>
    /// <param name="environmentName">The name of the environment to use 
    /// for the operation.</param>
    /// <param name="source">The source configuration to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a dictionary
    /// of key-value-pairs.</returns>
    private async Task<Dictionary<string, string>> ReplaceTokensAsync(
        string applicationName,
        string? environmentName,
        IConfigurationRoot source,
        CancellationToken cancellationToken = default
        )
    {
        // Create a place to put the results.
        var dest = new Dictionary<string, string>();

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through (count) settings.",
            source.AsEnumerable().Count()
            );

        // Loop through the settings.
        foreach (var setting in source.AsEnumerable())
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Getting the key and value for a setting"
                );

            // Get the key and value.
            var key = setting.Key;
            var value = setting.Value ?? "";  
            
            try
            {
                // Sanity check the value.
                if (!string.IsNullOrEmpty(value))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Looking for a replacement token in the setting value"
                        );

                    // Parse the token.
                    if (!ReplacementToken.TryParse(
                        value,
                        out var secretTag,
                        out var cacheTag,
                        out var altKey
                        ))
                    {
                        // Log what we are about to do.
                        _logger.LogWarning(
                            "Found malformed replacement token for application " +
                            "{app}, environment: {env}",
                            applicationName,
                            environmentName
                            );

                        // Panic!!
                        throw new InvalidDataException(
                            $"The replacement toke nwas invalid!"
                            );
                    }

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Checking for a cache tag"
                        );

                    // Is there a cache tag?
                    ProviderModel? cacheProvider = null;
                    if (!string.IsNullOrEmpty(cacheTag))
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Looking for provider with tag: {tag}",
                            cacheTag
                            );

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

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Checking for a cache provider"
                        );

                    // Is there a cache provider?
                    ICacheProcessor? cacheProcessor = null;
                    if (cacheProvider is not null)
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Checking provider enabled state"
                            );

                        // Is the cache provider enabled?
                        if (!cacheProvider.IsDisabled)
                        {
                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Creating a processor from the factory"
                                );

                            // Get the cache processor.
                            cacheProcessor = await _processorFactory.CreateCacheProcessorAsync(
                                cacheProvider,
                                cancellationToken
                                ).ConfigureAwait(false);

                            // Did we succeed?
                            if (cacheProcessor is not null)
                            {
                                // Log what we are about to do.
                                _logger.LogDebug(
                                    "Checking for a value in the cache"
                                    );

                                // Try to find a previously cached value.
                                value = await cacheProcessor.GetValueAsync(
                                    cacheProvider,
                                    $"{applicationName}:{environmentName ?? "none"}:{altKey ?? key}",
                                    cancellationToken
                                    ).ConfigureAwait(false);
                            }
                        }
                    }

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Checking for a value"
                        );

                    // Do we still need the secret value?
                    if (string.IsNullOrEmpty(value))
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Looking for provider with tag: {tag}",
                            secretTag
                            );

                        // Look for a matching provider.
                        var secretProvider = await _providerManager.FindByTagAndTypeAsync(
                            secretTag ?? "",
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

                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Checking for a secret provider"
                            );

                        // Is there a secret provider?
                        if (secretProvider is not null)
                        {
                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Checking provider enabled state"
                                );

                            // Is the secret provider enabled?
                            if (!secretProvider.IsDisabled)
                            {
                                // Log what we are about to do.
                                _logger.LogDebug(
                                    "Creating a processor from the factory"
                                    );

                                // Get the secret processor.
                                var secretProcessor = await _processorFactory.CreateSecretProcessorAsync(
                                    secretProvider,
                                    cancellationToken
                                    ).ConfigureAwait(false);

                                // Did we succeed?
                                if (secretProcessor is not null)
                                {
                                    // Log what we are about to do.
                                    _logger.LogDebug(
                                        "Fetching a secret value from a remote service"
                                        );

                                    // Try to find the value.
                                    value = await secretProcessor.GetValueAsync(
                                        secretProvider,
                                        altKey ?? key,
                                        cancellationToken
                                        ).ConfigureAwait(false);
                                }
                            }
                            else
                            {
                                // Log what we are about to do.
                                _logger.LogDebug(
                                    "Clearing value because the secret provider was disabled"
                                    );

                                // No value if the provider is disabled.
                                value = "";
                            }
                        }

                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Checking for a cache provider"
                            );

                        // Is there a cache provider?
                        if (cacheProvider is not null)
                        {
                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Checking provider enabled state"
                                );

                            // Is the cache provider enabled?
                            if (!cacheProvider.IsDisabled)
                            {
                                // Log what we are about to do.
                                _logger.LogDebug(
                                    "Checking for a cache processor"
                                    );

                                // Is there a cache processor?
                                if (cacheProcessor is not null)
                                {
                                    // Log what we are about to do.
                                    _logger.LogDebug(
                                        "Caching a value"
                                        );

                                    // Try to cache the value for next time.
                                    await cacheProcessor.SetValueAsync(
                                        cacheProvider,
                                        $"{applicationName}:{environmentName ?? "none"}:{altKey ?? key}",
                                        value ?? "",
                                        cancellationToken
                                        ).ConfigureAwait(false);
                                }
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

                // No value if we fail processing.
                value = "";
            }
            
            // Add the setting.
            dest.Add(key, value ?? "");
        }

        // Return the results.
        return dest;
    }

    #endregion
}
