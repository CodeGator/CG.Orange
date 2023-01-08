
using static CG.Orange.Globals.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CG.Orange.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="ISettingFileManager"/>
/// interface.
/// </summary>
internal class SettingFileManager : ISettingFileManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the cryptographer for this manager.
    /// </summary>
    internal protected readonly ICryptographer _cryptographer = null!;

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly ISettingFileRepository _repository = null!;

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
    /// <param name="cryptographer">The cryptographer to use with this
    /// manager.</param>
    /// <param name="settingFileRepository">The setting file repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public SettingFileManager(
        ICryptographer cryptographer,
        ISettingFileRepository settingFileRepository,
        ILogger<ISettingFileManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(cryptographer, nameof(cryptographer))
            .ThrowIfNull(settingFileRepository, nameof(settingFileRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _cryptographer = cryptographer;
        _repository = settingFileRepository;
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
            return await _repository.AnyAsync(
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
            return await _repository.AnyAsync(
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
            return await _repository.CountAsync(
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

            // Is there JSON?
            if (!string.IsNullOrEmpty(settingFile.Json))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Encrypting json for setting file: {id}",
                    settingFile.Id
                    );

                // Encrypt the JSON, at rest.
                settingFile.Json = await _cryptographer.AesEncryptAsync(
                    settingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Set non-null JSON.
                settingFile.Json = "{ }";
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.CreateAsync)
                );

            // Perform the operation.
            var newSettingFile = await _repository.CreateAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);

            // Is there JSON?
            if (!string.IsNullOrEmpty(newSettingFile.Json))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting JSON for setting file: {id}",
                    settingFile.Id
                    );

                // Decrypt the JSON.
                newSettingFile.Json = await _cryptographer.AesDecryptAsync(
                    newSettingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting empty JSON for setting file: {id}",
                    settingFile.Id
                    );

                // Set non-null JSON.
                newSettingFile.Json = "{ }";
            }

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
            await _repository.DeleteAsync(
                settingFile,
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
                "Updating the {name} model stats for: {id}",
                nameof(SettingFileModel),
                settingFile.Id
                );

            // Ensure the stats are correct.
            settingFile.LastUpdatedOnUtc = DateTime.UtcNow;
            settingFile.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the properties for setting file: {id}",
                settingFile.Id
                );

            // Disable the settingFile.
            settingFile.IsDisabled = true;

            // Is there JSON?
            if (!string.IsNullOrEmpty(settingFile.Json))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Encrypting json for setting file: {id}",
                    settingFile.Id
                    );

                // Encrypt the JSON.
                settingFile.Json = await _cryptographer.AesEncryptAsync(
                    settingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting empty JSON for setting file: {id}",
                    settingFile.Id
                    );

                // Set non-null JSON.
                settingFile.Json = "{ }";
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            var updatedSettingFile = await _repository.UpdateAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);

            // Is there JSON?
            if (string.IsNullOrEmpty(updatedSettingFile.Json))
            {
                // Decrypt the JSON.
                updatedSettingFile.Json = await _cryptographer.AesDecryptAsync(
                    updatedSettingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Set non-null JSON.
                updatedSettingFile.Json = "{ }";
            }

            // Return the results.
            return updatedSettingFile;
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
                "Updating the {name} model stats for: {id}",
                nameof(SettingFileModel),
                settingFile.Id
                );

            // Ensure the stats are correct.
            settingFile.LastUpdatedOnUtc = DateTime.UtcNow;
            settingFile.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the properties for setting file: {id}",
                settingFile.Id
                );

            // Enable the settingFile.
            settingFile.IsDisabled = false;

            // Is there JSON?
            if (!string.IsNullOrEmpty(settingFile.Json))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Encrypting json for setting file: {id}",
                    settingFile.Id
                    );

                // Encrypt the JSON.
                settingFile.Json = await _cryptographer.AesEncryptAsync(
                    settingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting empty JSON for setting file: {id}",
                    settingFile.Id
                    );

                // Set non-null JSON.
                settingFile.Json = "{ }";
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            var updatedSettingFile = await _repository.UpdateAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);

            // Is there JSON?
            if (string.IsNullOrEmpty(updatedSettingFile.Json))
            {
                // Decrypt the JSON.
                updatedSettingFile.Json = await _cryptographer.AesDecryptAsync(
                    updatedSettingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Set non-null JSON.
                updatedSettingFile.Json = "{ }";
            }

            // Return the results.
            return updatedSettingFile;
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
            var data = await _repository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // We must create the secondary list here because the loop
            //   below creates temporary objects inside the enumeration
            //   process, so, the work we do to decrypt the JSON gets
            //   lost unless we manually copy the results to another list.
            var result = new List<SettingFileModel>();

            // Loop through the setting files.
            foreach (var settingFile in data)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting JSON for setting file: {key}",
                    settingFile.Id
                    );

                // Is there JSON?
                if (!string.IsNullOrEmpty(settingFile.Json))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting JSON for setting file: {id}",
                        settingFile.Id
                        );

                    // Decrypt the JSON.
                    settingFile.Json = await _cryptographer.AesDecryptAsync(
                        settingFile.Json,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Setting empty JSON for setting file: {id}",
                        settingFile.Id
                        );

                    // Set non-null JSON.
                    settingFile.Json = "{ }";
                }

                // Add to the list.
                result.Add(settingFile);
            }

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
            var data = await _repository.FindByApplicationAndEnvironmentAsync(
                applicationName,
                environmentName,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we find a match?
            if (data is not null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting JSON for setting file: {key}",
                    data.Id
                    );

                // Is there JSON?
                if (!string.IsNullOrEmpty(data.Json))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting JSON for setting file: {id}",
                        data.Id
                        );

                    // Decrypt the JSON.
                    data.Json = await _cryptographer.AesDecryptAsync(
                        data.Json,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Setting empty JSON for setting file: {id}",
                        data.Id
                        );

                    // Set non-null JSON.
                    data.Json = "{ }";
                }
            }

            // Return the results.
            return data;
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
            var result = await _repository.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we find a match?
            if (result is not null)
            {
                // Is there JSON?
                if (!string.IsNullOrEmpty(result.Json))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting JSON for setting file: {id}",
                        result.Id
                        );

                    // Decrypt the JSON.
                    result.Json = await _cryptographer.AesDecryptAsync(
                        result.Json,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Setting empty JSON for setting file: {id}",
                        result.Id
                        );

                    // Set non-null JSON.
                    result.Json = "{ }";
                }
            }

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

            // Is there JSON?
            if (!string.IsNullOrEmpty(settingFile.Json))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Encrypting json for setting file: {id}",
                    settingFile.Id
                    );

                // Encrypt the JSON, at rest.
                settingFile.Json = await _cryptographer.AesEncryptAsync(
                    settingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting empty JSON for setting file: {id}",
                    settingFile.Id
                    );

                // Set non-null JSON.
                settingFile.Json = "{ }";
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ISettingFileRepository.UpdateAsync)
                );

            // Perform the operation.
            var updatedSettingFile = await _repository.UpdateAsync(
                settingFile,
                cancellationToken
                ).ConfigureAwait(false);

            // Is there a value?
            if (!string.IsNullOrEmpty(updatedSettingFile.Json))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting JSON for setting file: {id}",
                    updatedSettingFile.Id
                    );

                // Decrypt the JSON.
                updatedSettingFile.Json = await _cryptographer.AesDecryptAsync(
                    updatedSettingFile.Json,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting empty JSON for setting file: {id}",
                    updatedSettingFile.Id
                    );

                // Set non-null JSON.
                updatedSettingFile.Json = "{ }";
            }

            // Return the results.
            return updatedSettingFile;

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
