
namespace CG.Orange.Managers;

internal class SettingFileCountManager : ISettingFileCountManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly ISettingFileCountRepository _settingFileCountRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<ISettingFileCountManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileCountManager"/>
    /// class.
    /// </summary>
    /// <param name="settingFileCountRepository">The setting file repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public SettingFileCountManager(
        ISettingFileCountRepository settingFileCountRepository,
        ILogger<ISettingFileCountManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFileCountRepository, nameof(settingFileCountRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _settingFileCountRepository = settingFileCountRepository;
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
                nameof(ISettingFileCountRepository.AnyAsync)
                );

            // Perform the search.
            return await _settingFileCountRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for setting file counts!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting file counts!",
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
                nameof(ISettingFileCountRepository.CountAsync)
                );

            // Perform the search.
            return await _settingFileCountRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count setting file counts!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count setting file counts!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<IEnumerable<SettingFileCountModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileCountRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _settingFileCountRepository.FindAllAsync(
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
                "Failed to search for setting file counts!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting file counts!",
                innerException: ex
                );
        }
    }

    #endregion
}
