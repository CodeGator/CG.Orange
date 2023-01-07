
namespace CG.Orange.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IConfigurationEventManager"/>
/// interface.
/// </summary>
internal class ConfigurationEventManager : IConfigurationEventManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IConfigurationEventRepository _repository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IConfigurationEventManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ConfigurationEventManager"/>
    /// class.
    /// </summary>
    /// <param name="configurationEventRepository">The setting file repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ConfigurationEventManager(
        IConfigurationEventRepository configurationEventRepository,
        ILogger<IConfigurationEventManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(configurationEventRepository, nameof(configurationEventRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _repository = configurationEventRepository;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc />
    public virtual async Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IConfigurationEventRepository.AnyAsync)
                );

            // Perform the search.
            return await _repository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for configuration events!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for configuration events!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ConfigurationEventModel> CreateAsync(
        ConfigurationEventModel configurationEvent,
        TimeSpan maxHistory,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configurationEvent, nameof(configurationEvent))
            .ThrowIfZero(maxHistory, nameof(maxHistory));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(SettingFileModel)
                );

            // Ensure the stats are correct.
            configurationEvent.CreatedOnUtc = DateTime.UtcNow;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.CreateAsync)
                );

            // Perform the operation.
            var newConfigurationEvent = await _repository.CreateAsync(
                configurationEvent,
                cancellationToken
                ).ConfigureAwait(false);

            // Remove any outdated history.
            await _repository.DeleteOlderThanAsync(
                maxHistory,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return newConfigurationEvent;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a configuration event!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for configuration event!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<int> CountAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IConfigurationEventRepository.CountAsync)
                );

            // Perform the search.
            return await _repository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count configuration events!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count configuration events!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<IEnumerable<ConfigurationEventModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IConfigurationEventRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _repository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for configuration events!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for configuration events!",
                innerException: ex
                );
        }
    }

    #endregion
}
