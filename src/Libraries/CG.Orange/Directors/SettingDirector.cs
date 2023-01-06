
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="ISettingDirector"/>
/// interface.
/// </summary>
internal class SettingDirector : ISettingDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the setting file manager for this director.
    /// </summary>
    internal protected readonly ISettingFileManager _settingFileManager = null!;

    /// <summary>
    /// This field contains the setting file count manager for this director.
    /// </summary>
    internal protected readonly ISettingFileCountManager _settingFileCountManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<ISettingDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingDirector"/>
    /// class.
    /// </summary>
    /// <param name="settingFileManager">The setting file manager to use
    /// with this director.</param>
    /// <param name="settingFileCountManager">The setting file count 
    /// manager to use with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public SettingDirector(
        ISettingFileManager settingFileManager,
        ISettingFileCountManager settingFileCountManager,
        ILogger<ISettingDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFileManager, nameof(settingFileManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _settingFileManager = settingFileManager;   
        _settingFileCountManager = settingFileCountManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Defer to the manager.
            return await _settingFileManager.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find setting files!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(
        string applicationName,
        string? environmentName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(applicationName, nameof(applicationName));

        try
        {
            // Defer to the manager.
            return await _settingFileManager.AnyAsync(
                applicationName,
                environmentName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find setting files!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Defer to the manager.
            return await _settingFileManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count setting files!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to count setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel> CreateAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Defer to the manager for the create.
            var newSettingFile = await _settingFileManager.CreateAsync(
                settingFile,
                userName,
                cancellationToken
                ).ConfigureAwait(false);

            // Get a count of the setting files.
            var count = await _settingFileCountManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Defer to the manager for the event.
            await _settingFileCountManager.CreateAsync(
                new SettingFileCountModel() { Count = count },
                userName,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return newSettingFile;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a setting file!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to create a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Defer to the manager.
            await _settingFileManager.DeleteAsync(
                settingFile,
                userName,
                cancellationToken
                ).ConfigureAwait(false);

            // Get a count of the setting files.
            var count = await _settingFileCountManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Defer to the manager for the event.
            await _settingFileCountManager.CreateAsync(
                new SettingFileCountModel() { Count = count },
                userName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a setting file!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to delete a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel> DisableAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Defer to the manager.
            return await _settingFileManager.DisableAsync(
                settingFile,
                userName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to disable a setting file!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to disable a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel> EnableAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Defer to the manager.
            return await _settingFileManager.EnableAsync(
                settingFile,
                userName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to enable a setting file!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to enable a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<SettingFileModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Defer to the manager.
            return await _settingFileManager.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find setting files!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel?> FindByApplicationAndEnvironmentAsync(
        string applicationName,
        string? environmentName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(applicationName, nameof(applicationName));

        try
        {
            // Defer to the manager.
            return await _settingFileManager.FindByApplicationAndEnvironmentAsync(
                applicationName,
                environmentName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find setting files by tag and type!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find setting files " +
                "by tag and type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfZero(id, nameof(id));

        try
        {
            // Defer to the manager.
            return await _settingFileManager.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find setting files by id!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find setting files " +
                "by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel> UpdateAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Defer to the manager.
            return await _settingFileManager.UpdateAsync(
                settingFile,
                userName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find setting files by id!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find setting files " +
                "by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    #endregion
}
