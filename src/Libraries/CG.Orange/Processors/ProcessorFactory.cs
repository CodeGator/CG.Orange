
using System.Reflection;

namespace CG.Orange.Processors;

/// <summary>
/// This class is a default implementation of the <see cref="IProcessorFactory"/>
/// interface.
/// </summary>
internal class ProcessorFactory : IProcessorFactory
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the service provider for this factory.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// This field contains the provider manager for this factory.
    /// </summary>
    internal protected readonly IProviderManager _providerManager;  

    /// <summary>
    /// This field contains the logger for this factory.
    /// </summary>
    internal protected readonly ILogger<IProcessorFactory> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProcessorFactory"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with 
    /// this factory.</param>
    /// <param name="providerManager">The provider manager to use with 
    /// this factory.</param>
    /// <param name="logger">The logger to use with this factory.</param>
    public ProcessorFactory(
        IServiceProvider serviceProvider,
        IProviderManager providerManager,
        ILogger<IProcessorFactory> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(providerManager, nameof(providerManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _serviceProvider = serviceProvider;
        _providerManager = providerManager; 
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual Task<ICacheProcessor?> CreateCacheProcessorAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Sanity check the provider type.
            if (provider.ProviderType != ProviderType.Cache)
            {
                // Panic!!
                throw new InvalidOperationException(
                    $"The provider isn't a cache provider!"
                    );
            }

            // Sanity check the processor type.
            if (string.IsNullOrEmpty(provider.ProcessorType))
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The provider processor tag was missing!"
                    );
            }

            // For some reason known only to .NET, the types in our plugins are not
            //   visible to the Type.GetType method. So, we have to resort to this
            //   in order to load the .NET type for our processor (which lives
            //   inside one of the plugins).

            // Attempt to get the original plugin name.
            var pluginName = provider.ProcessorType.Substring(
                0,
                provider.ProcessorType.LastIndexOf('.')
                );

            // Load the assembly for the plugin.
            var asm = Assembly.Load(pluginName);

            // Load the type for the processor.
            var processorType = asm.GetType(
                provider.ProcessorType
                );

            // Sanity check the type.
            if (processorType is null) 
            {
                // Log what happened.
                _logger.LogWarning(
                    "Failed to resolve .NET type: {type} for a processor!",
                    provider.ProcessorType
                    );
                return Task.FromResult<ICacheProcessor?>(null); // Failed!
            }

            // Attempt to create the processor.
            var processor = _serviceProvider.GetRequiredService(
                processorType
                ) as ICacheProcessor;

            // Return the results.
            return Task.FromResult(processor);
        }
        catch (Exception ex ) 
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a cache processor for provider: {prov}!",
                provider.Name
                );

            // Provide better context.
            throw new ManagerException(
                message: $"The factory failed to create a cache processor " +
                $"for provider: {provider.Name}!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual Task<ISecretProcessor?> CreateSecretProcessorAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Sanity check the provider type.
            if (provider.ProviderType != ProviderType.Secret)
            {
                // Panic!!
                throw new InvalidOperationException(
                    $"The provider isn't a secret provider!"
                    );
            }

            // Sanity check the processor type.
            if (string.IsNullOrEmpty(provider.ProcessorType))
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The provider processor type was missing!"
                    );
            }

            // Sanity check the processor type.
            if (!provider.ProcessorType.Contains('.'))
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The provider processor type was malformed!"
                    );
            }

            // For some reason known only to .NET, the types in our plugins are not
            //   visible to the Type.GetType method. So, we have to resort to this
            //   in order to load the .NET type for our processor (which lives
            //   inside one of the plugins).

            // Attempt to get the original plugin name.
            var pluginName = provider.ProcessorType.Substring(
                0,
                provider.ProcessorType.LastIndexOf('.')
                );

            // Load the assembly for the plugin.
            var asm = Assembly.Load(pluginName);

            // Load the type for the processor.
            var processorType = asm.GetType(
                provider.ProcessorType
                );
            
            // Sanity check the type.
            if (processorType is null)
            {
                // Log what happened.
                _logger.LogWarning(
                    "Failed to resolve .NET type: {type} for a processor!",
                    provider.ProcessorType
                    );
                return Task.FromResult<ISecretProcessor?>(null); // Failed!
            }

            // Attempt to create the processor.
            var processor = _serviceProvider.GetRequiredService(
                processorType
                ) as ISecretProcessor;

            // Return the results.
            return Task.FromResult(processor);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a secret processor for provider: {prov}!",
                provider.Name
                );

            // Provide better context.
            throw new ManagerException(
                message: $"The factory failed to create a secret processor " +
                $"for provider: {provider.Name}!",
                innerException: ex
                );
        }
    }

    #endregion
}
