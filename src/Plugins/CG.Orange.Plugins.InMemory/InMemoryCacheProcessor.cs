
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
    /// This field contains the shared table of values.
    /// </summary>
    internal protected readonly ConcurrentDictionary<string, string> _table = new();

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
    /// <param name="logger">The logger to use with this processor.</param>
    public InMemoryCacheProcessor(
        ILogger<InMemoryCacheProcessor> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual Task<string?> GetValueAsync(
        string key,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(key, nameof(key));

        try
        {
            // Try to get the value.
            if (!_table.TryGetValue(key, out var value))
            {
                // Log what happened.
                _logger.LogInformation(
                    "Failed to locate key: {key} from the cache",
                    key
                    );
                return Task.FromResult<string?>(null);
            }

            // Return the results.
            return Task.FromResult<string?>(value);
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
    public virtual Task SetValueAsync(
        string key,
        string value,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(key, nameof(key));

        try
        {
            // Try to add the value.
            if (!_table.TryAdd(key, value))
            {
                // Try to update the value.
                _table.TryUpdate(key, value, "");
            }

            // Return the task.
            return Task.CompletedTask;
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
