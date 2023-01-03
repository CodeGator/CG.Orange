﻿
namespace CG.Orange.Plugins.InMemory;

/// <summary>
/// This class is an in-memory implementation of the <see cref="ICacheProcessor"/>
/// interface.
/// </summary>
internal class InMemoryCacheProcessor : ICacheProcessor
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the cache for this processor.
    /// </summary>
    internal protected readonly IDistributedCache _cache;

    /// <summary>
    /// This field contains the logger for this processor. 
    /// </summary>
    internal protected readonly ILogger<InMemoryCacheProcessor> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="InMemoryCacheProcessor"/>
    /// class.
    /// </summary>
    /// <param name="cache">The cache to use with this processor.</param>
    /// <param name="logger">The logger to use with this processor.</param>
    public InMemoryCacheProcessor(
        IDistributedCache cache,
        ILogger<InMemoryCacheProcessor> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(cache, nameof(cache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _cache = cache;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<string?> GetValueAsync(
        ProviderModel provider,
        string key,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider))
            .ThrowIfNullOrEmpty(key, nameof(key));

        try
        {
            // Get the bytes from the cache.
            var bytes = await _cache.GetAsync(
                key,
                cancellationToken
                ).ConfigureAwait(false);

            // Covert the bytes to a string.
            var value = Encoding.UTF8.GetString(bytes);

            // Return the value.
            return value;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to retrieve a value for key: {key} from the cache!",
                key
                );

            // Provider better context for the error.
            throw new ProcessorException(
                innerException: ex,
                message: $"Failed to retrieve a value for key: {key} " +
                "from the cache!"
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SetValueAsync(
        ProviderModel provider,
        string key,
        string value,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider))
            .ThrowIfNullOrEmpty(key, nameof(key));

        try
        {
            // Look for the provider property.
            var duration = provider.Properties.FirstOrDefault(x =>
                string.Compare(x.Key, "duration", StringComparison.InvariantCultureIgnoreCase) == 0
                );

            // Did we fail?
            if (duration is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The provider property 'Duration' was not found for provider: {provider.Id}"
                    );
            }

            // Calculate the expiration date/time.
            var expiration = !string.IsNullOrEmpty(duration.Value)
                ? TimeSpan.Parse(duration.Value)
                : TimeSpan.Zero;  

            // Convert the value to bytes.
            var bytes = Encoding.UTF8.GetBytes(value);

            // Set the bytes in the cache.
            await _cache.SetAsync(
                key,
                bytes,
                new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = expiration
                },
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to set a value for key: {key} from the cache!",
                key
                );

            // Provider better context for the error.
            throw new ProcessorException(
                innerException: ex,
                message: $"Failed to set a value for key: {key} " +
                "from the cache!"
                );
        }
    }

    #endregion
}
