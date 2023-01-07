
namespace CG.Orange.Data.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IConfigurationEventRepository"/>
/// interface.
/// </summary>
internal class ConfigurationEventRepository : IConfigurationEventRepository
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
    internal protected readonly ILogger<IConfigurationEventRepository> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ConfigurationEventRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContext">The EFCORE data-context to use with this 
    /// repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public ConfigurationEventRepository(
        OrangeDbContext dbContext,
        IMapper mapper,
        ILogger<IConfigurationEventRepository> logger
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
                "Searching for configuration events"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.ConfigurationEvents.AnyAsync(
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
                "Failed to search for configuration events!"
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
            var data = await _dbContext.ConfigurationEvents.CountAsync(
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
                "Failed to count configuration events!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count configuration " +
                "events!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ConfigurationEventModel> CreateAsync(
        ConfigurationEventModel configurationEvent,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configurationEvent, nameof(configurationEvent));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(ConfigurationEventModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ConfigurationEventEntity>(
                configurationEvent
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ConfigurationEventModel)} " +
                    "model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(ConfigurationEventEntity),
                nameof(OrangeDbContext)
                );

            // Add the entity to the data-store.
            _dbContext.ConfigurationEvents.Attach(entity);

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
                nameof(ConfigurationEventEntity)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ConfigurationEventModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ConfigurationEventEntity)} " +
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
                "Failed to create a configuration event!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a configuration " +
                "event!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteOlderThanAsync(
        TimeSpan maxHistory,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfZero(maxHistory, nameof(maxHistory));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for configuration events."
                );

            // Calculate a cutoff date.
            var cutoffDate = DateTime.UtcNow.Subtract(maxHistory);

            // Perform the event search.
            var configurationEvents = await _dbContext.ConfigurationEvents.Where(
                x=> x.CreatedOnUtc < cutoffDate
                ).ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Removing {count} outdated configuration events.",
                configurationEvents.Count
                );

            // Remove the events.
            _dbContext.ConfigurationEvents.RemoveRange(
                configurationEvents
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
                "Failed to remove outdated configuration events!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to remove outdated " +
                "configuration events!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ConfigurationEventModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for configuration events."
                );

            // Perform the configuration search.
            var configurationEvents = await _dbContext.ConfigurationEvents
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = configurationEvents.Select(x =>
                _mapper.Map<ConfigurationEventModel>(x)
                );

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
            throw new RepositoryException(
                message: $"The repository failed to search for " +
                "configuration events!",
                innerException: ex
                );
        }
    }

    #endregion
}
