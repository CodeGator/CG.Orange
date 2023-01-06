
namespace CG.Orange.Data.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="ISettingFileRepository"/>
/// interface.
/// </summary>
internal class SettingFileRepository : ISettingFileRepository
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the EFCORE data-context for this repository.
    /// </summary>
    internal protected readonly OrangeDbContext _dbContext = null!;

    /// <summary>
    /// This field contains the auto-mapper for this repository.
    /// </summary>
    internal protected readonly IMapper _mapper = null!;

    /// <summary>
    /// This field contains the logger for this repository.
    /// </summary>
    internal protected readonly ILogger<ISettingFileRepository> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContext">The EFCORE data-context to use with this 
    /// repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public SettingFileRepository(
        OrangeDbContext dbContext,
        IMapper mapper,
        ILogger<ISettingFileRepository> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(dbContext, nameof(dbContext))
            .ThrowIfNull(mapper, nameof(mapper))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _dbContext = dbContext;
        _mapper = mapper;
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for setting files"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.SettingFiles.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return data;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for setting files!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for setting files!",
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
        Guard.Instance().ThrowIfNull(applicationName, nameof(applicationName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for setting files"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.SettingFiles.AnyAsync(x =>
                x.ApplicationName == applicationName &&
                x.EnvironmentName == environmentName,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return data;
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
            throw new RepositoryException(
                message: $"The repository failed to search for setting " +
                "files by application and environment name!",
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for setting files"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.SettingFiles.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return data;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count setting files!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count setting files!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel> CreateAsync(
        SettingFileModel settingFile,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(SettingFileModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.SettingFileEntity>(
                settingFile
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(SettingFileModel)} model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(SettingFileEntity),
                nameof(OrangeDbContext)
                );

            // Add the entity to the data-store.
            _dbContext.SettingFiles.Attach(entity);

            // Mark the entity as added so EFCORE will insert it.
            _dbContext.Entry(entity).State = EntityState.Added;

            // Log what we are about to do.
            _logger.LogDebug(
                "Saving changes to the {ctx} data-context",
                nameof(OrangeDbContext)
                );

            // Save the changes.
            await _dbContext.SaveChangesAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(SettingFileEntity)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<SettingFileModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(SettingFileEntity)} " +
                    "entity to a model."
                    );
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create an setting file!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create an setting file!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        SettingFileModel settingFile,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "looking for the tracked {entity} instance from the {ctx} data-context",
                nameof(SettingFileModel),
                nameof(OrangeDbContext)
                );

            // Find the tracked entity (if any).
            var entity = await _dbContext.SettingFiles.FindAsync(
                settingFile.Id,
                cancellationToken
                );

            // Did we fail?
            if (entity is null)
            {
                return; // Nothing to do!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "deleting an {entity} instance from the {ctx} data-context",
                nameof(SettingFileModel),
                nameof(OrangeDbContext)
                );

            // Delete from the data-store.
            _dbContext.SettingFiles.Remove(
                entity
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Saving changes to the {ctx} data-context",
                nameof(OrangeDbContext)
                );

            // Save the changes.
            await _dbContext.SaveChangesAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete an setting file!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete an setting file!",
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for setting files."
                );

            // Perform the setting file search.
            var settingFiles = await _dbContext.SettingFiles
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = settingFiles.Select(x =>
                _mapper.Map<SettingFileModel>(x)
                );

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
            throw new RepositoryException(
                message: $"The repository failed to search for " +
                "setting files!",
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
        Guard.Instance().ThrowIfNull(applicationName, nameof(applicationName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for a setting file."
                );

            // Perform the setting file search.
            var settingFile = await _dbContext.SettingFiles.Where(x =>
                x.ApplicationName == applicationName &&
                x.EnvironmentName == environmentName
                ).AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (settingFile is null)
            {
                return null;
            }

            // Convert the entities to a models.
            var result = _mapper.Map<SettingFileModel>(settingFile);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a setting file by application " +
                "and environment!"
                );

            // Provider better context.
            throw new RepositoryException( 
                message: $"The repository failed to search for a setting " +
                "file by application and environment!",
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for a setting file."
                );

            // Perform the setting file search.
            var settingFile = await _dbContext.SettingFiles.Where(x =>
                x.Id == id
                ).AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (settingFile is null)
            {
                return null;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(SettingFileEntity)
                );

            // Convert the entities to a models.
            var result = _mapper.Map<SettingFileModel>(settingFile);

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(SettingFileEntity)} " +
                    "entity to a model."
                    );
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a setting file by id"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a setting " +
                "file by id",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileModel> UpdateAsync(
        SettingFileModel settingFile,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFile, nameof(settingFile));

        try
        {
            // Look for the given provider.
            var entity = await _dbContext.SettingFiles.Where(x =>
                x.Id == settingFile.Id
                ).FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The setting file: {settingFile.Id} was not found!"
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(SettingFileModel),
                nameof(OrangeDbContext)
                );

            // Update the editable properties.
            entity.IsDisabled = settingFile.IsDisabled;
            entity.Json = settingFile.Json;
            entity.EnvironmentName = settingFile.EnvironmentName;
            entity.ApplicationName = settingFile.ApplicationName;
            entity.LastUpdatedOnUtc = settingFile.LastUpdatedOnUtc;
            entity.LastUpdatedBy = settingFile.LastUpdatedBy;

            // We never change these 'read only' properties.
            _dbContext.Entry(entity).Property(x => x.Id).IsModified = false;
            _dbContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            _dbContext.Entry(entity).Property(x => x.CreatedOnUtc).IsModified = false;

            // Log what we are about to do.
            _logger.LogDebug(
                "Saving changes to the {ctx} data-context",
                nameof(OrangeDbContext)
                );

            // Save the changes.
            await _dbContext.SaveChangesAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(SettingFileEntity)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<SettingFileModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(SettingFileEntity)} " +
                    "entity to a model."
                    );
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update an setting file!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update an setting file!",
                innerException: ex
                );
        }
    }

    #endregion
}
