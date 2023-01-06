
using Microsoft.EntityFrameworkCore;
using static CG.Orange.Globals.Models;

namespace CG.Orange.Data.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="ISettingFileCountRepository"/>
/// interface.
/// </summary>
internal class SettingFileCountRepository : ISettingFileCountRepository
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
    internal protected readonly ILogger<ISettingFileCountRepository> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileCountRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContext">The EFCORE data-context to use with this 
    /// repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public SettingFileCountRepository(
        OrangeDbContext dbContext,
        IMapper mapper,
        ILogger<ISettingFileCountRepository> logger
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
                "Searching for setting file counts"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.SettingFileCounts.AnyAsync(
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
                "Failed to search for setting file counts!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for setting " +
                "file counts!",
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
            var data = await _dbContext.SettingFileCounts.CountAsync(
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
                "Failed to count setting file counts!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count setting file " +
                "counts!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<SettingFileCountModel> CreateAsync(
        SettingFileCountModel settingFileCount,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFileCount, nameof(settingFileCount));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(SettingFileCountModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.SettingFileCountEntity>(
                settingFileCount
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(SettingFileCountModel)} " +
                    "model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(SettingFileCountEntity),
                nameof(OrangeDbContext)
                );

            // Add the entity to the data-store.
            _dbContext.SettingFileCounts.Attach(entity);

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
                nameof(SettingFileCountEntity)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<SettingFileCountModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(SettingFileCountEntity)} " +
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
                "Failed to create a setting file count!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a setting " +
                "file count!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<SettingFileCountModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for setting file counts."
                );

            // Perform the setting file search.
            var settingFileCounts = await _dbContext.SettingFileCounts
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = settingFileCounts.Select(x =>
                _mapper.Map<SettingFileCountModel>(x)
                );

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
            throw new RepositoryException(
                message: $"The repository failed to search for " +
                "setting file counts!",
                innerException: ex
                );
        }
    }

    #endregion
}
