﻿
using CG.Orange.Repositories;

namespace CG.Orange.Managers;

internal class SettingFileManager : ISettingFileManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the setting file repository for this manager.
    /// </summary>
    internal protected readonly ISettingFileRepository _settingFileRepository = null!;

    /// <summary>
    /// This field contains the setting file count repository for this manager.
    /// </summary>
    internal protected readonly ISettingFileCountRepository _settingFileCountRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<ISettingFileManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileManager"/>
    /// class.
    /// </summary>
    /// <param name="settingFileRepository">The setting file repository to use
    /// with this manager.</param>
    /// <param name="settingFileCountRepository">The setting file count 
    /// repository to use with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public SettingFileManager(
        ISettingFileRepository settingFileRepository,
        ISettingFileCountRepository settingFileCountRepository,
        ILogger<ISettingFileManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFileRepository, nameof(settingFileRepository))
            .ThrowIfNull(settingFileCountRepository, nameof(settingFileCountRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _settingFileRepository = settingFileRepository;
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
                nameof(ISettingFileRepository.AnyAsync)
                );

            // Perform the search.
            return await _settingFileRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for setting files!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.AnyAsync)
                );

            // Perform the search.
            return await _settingFileRepository.AnyAsync(
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
                "Failed to search for setting files by application and " +
                "environment name!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting " +
                "files by application and environment name!",
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
                nameof(ISettingFileRepository.CountAsync)
                );

            // Perform the search.
            return await _settingFileRepository.CountAsync(
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
            throw new ManagerException(
                message: $"The manager failed to count setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(SettingFileModel)
                );

            // Ensure the stats are correct.
            settingFile.CreatedOnUtc = DateTime.UtcNow;
            settingFile.CreatedBy = userName;
            settingFile.LastUpdatedBy = null;
            settingFile.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.CreateAsync)
                );

            // Perform the operation.
            var newSettingFile = await _settingFileRepository.CreateAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogTrace(
                "Counting setting files"
                );

            // Get the count of setting files.
            var newCount = await _settingFileRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogTrace(
                "Recording the new count of setting files"
                );

            // Record the new count.
            await _settingFileCountRepository.CreateAsync(
                newCount
                ).ConfigureAwait(false);

            // Return the results.
            return newSettingFile;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new setting file!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(SettingFileModel)
                );

            // Ensure the stats are correct.
            settingFile.LastUpdatedOnUtc = DateTime.UtcNow;
            settingFile.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.DeleteAsync)
                );

            // Perform the operation.
            await _settingFileRepository.DeleteAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogTrace(
                "Counting setting files"
                );

            // Get the count of setting files.
            var newCount = await _settingFileRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogTrace(
                "Recording the new count of setting files"
                );

            // Record the new count.
            await _settingFileCountRepository.CreateAsync(
                newCount
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
            throw new ManagerException(
                message: $"The manager failed to delete a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Can we take a shortcut?
            if (settingFile.IsDisabled)
            {
                return settingFile; // Nothing to do.
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(SettingFileModel)
                );

            // Ensure the stats are correct.
            settingFile.LastUpdatedOnUtc = DateTime.UtcNow;
            settingFile.LastUpdatedBy = userName;

            // Disable the settingFile.
            settingFile.IsDisabled = true;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _settingFileRepository.UpdateAsync(
                settingFile,
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
            throw new ManagerException(
                message: $"The manager failed to disable a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Can we take a shortcut?
            if (!settingFile.IsDisabled)
            {
                return settingFile; // Nothing to do.
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(SettingFileModel)
                );

            // Ensure the stats are correct.
            settingFile.LastUpdatedOnUtc = DateTime.UtcNow;
            settingFile.LastUpdatedBy = userName;

            // Enable the settingFile.
            settingFile.IsDisabled = false;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _settingFileRepository.UpdateAsync(
                settingFile,
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
            throw new ManagerException(
                message: $"The manager failed to enable a setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<IEnumerable<SettingFileModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _settingFileRepository.FindAllAsync(
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
                "Failed to search for setting files!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.FindByApplicationAndEnvironmentAsync)
                );

            // Perform the operation.
            var result = await _settingFileRepository.FindByApplicationAndEnvironmentAsync(
                applicationName,
                environmentName,
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
                "Failed to search for setting files by application and environment!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting files by " +
                "application and environment!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<SettingFileModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfZero(id, nameof(id));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.FindByIdAsync)
                );

            // Perform the operation.
            var result = await _settingFileRepository.FindByIdAsync(
                id,
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
                "Failed to search for setting files by id!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for setting files by " +
                "id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(SettingFileModel)
                );

            // Ensure the stats are correct.
            settingFile.LastUpdatedOnUtc = DateTime.UtcNow;
            settingFile.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _settingFileRepository.UpdateAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a settingFile!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a settingFile!",
                innerException: ex
                );
        }
    }

    #endregion
}
