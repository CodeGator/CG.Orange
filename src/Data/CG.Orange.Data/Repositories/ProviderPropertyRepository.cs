
using Microsoft.EntityFrameworkCore;

namespace CG.Orange.Data.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IProviderPropertyRepository"/>
/// interface.
/// </summary>
internal class ProviderPropertyRepository : IProviderPropertyRepository
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the EFCORE data-context for this repository.
    /// </summary>
    internal protected readonly OrangeDbContext _dbContext;

    /// <summary>
    /// This field contains the auto-mapper for this repository.
    /// </summary>
    internal protected readonly IMapper _mapper;

    /// <summary>
    /// This field contains the logger for this repository.
    /// </summary>
    internal protected readonly ILogger<IProviderPropertyRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderPropertyRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContext">The EFCORE data-context to use with this 
    /// repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public ProviderPropertyRepository(
        OrangeDbContext dbContext,
        IMapper mapper,
        ILogger<IProviderPropertyRepository> logger
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
                "Searching for providers"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.ProviderProperties.AnyAsync(
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
                "Failed to search for provider properties!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to search for provider " +
                "properties!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<bool> AnyByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for properties by provider"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.ProviderProperties.AnyAsync(x =>
                x.ProviderId == provider.Id,
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
                "Failed to search for properties by provider!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to search for properties " +
                "by provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<long> CountAsync(
       CancellationToken cancellationToken = default
       )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for provider properties"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.ProviderProperties.CountAsync(
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
                "Failed to count provider properties!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to count provider " +
                "properties!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<int> CountByProviderAsync(
        ProviderModel provider,
       CancellationToken cancellationToken = default
       )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for properties by provider"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.ProviderProperties.CountAsync(x =>
                x.ProviderId == provider.Id,
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
                "Failed to count properties by provider!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to count properties" +
                "by provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderPropertyModel> CreateAsync(
        ProviderPropertyModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(ProviderPropertyModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ProviderPropertyEntity>(
                provider
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderPropertyModel)} " +
                    "model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(ProviderPropertyModel),
                nameof(OrangeDbContext)
                );

            // Add the entity to the data-store.
            _dbContext.ProviderProperties.Attach(entity);

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
                nameof(ProviderPropertyModel)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ProviderPropertyModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderPropertyModel)} " +
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
                "Failed to create a provider property!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to create a provider " +
                "property!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        ProviderPropertyModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(ProviderPropertyModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ProviderPropertyEntity>(
                provider
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderPropertyModel)} " +
                    "model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "looking for the tracked {entity} instance from the {ctx} data-context",
                nameof(ProviderPropertyModel),
                nameof(OrangeDbContext)
                );

            // Find the tracked entity (if any).
            var trackedEntry = await _dbContext.ProviderProperties.FindAsync(
                entity.Id,
                cancellationToken
                );

            // Did we fail?
            if (trackedEntry is null)
            {
                return; // Nothing to do!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "deleting an {entity} instance from the {ctx} data-context",
                nameof(ProviderPropertyModel),
                nameof(OrangeDbContext)
                );

            // Delete from the data-store.
            _dbContext.ProviderProperties.Remove(
                trackedEntry
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
                "Failed to delete a provider property!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a provider " +
                "property",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ProviderPropertyModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for provider properties."
                );

            // Perform the provider search.
            var providers = await _dbContext.ProviderProperties
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = providers.Select(x =>
                _mapper.Map<ProviderPropertyModel>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider properties!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to search for " +
                "provider properties!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ProviderPropertyModel>> FindByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for a provider."
                );

            // Perform the provider search.
            var data = await _dbContext.ProviderProperties.Where(x =>
                x.ProviderId == provider.Id
                ).AsNoTracking()
                .ToListAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (data is null)
            {
                return new List<ProviderPropertyModel>();
            }

            // Convert the entities to a models.
            var result = data.Select(x => 
                _mapper.Map<ProviderPropertyModel>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for properties by provider!"
                );

            // ProviderProperty better context.
            throw new RepositoryException( 
                message: $"The repository failed to search for properties " +
                "by provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderPropertyModel?> FindByIdAsync(
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
                "Searching for a provider."
                );

            // Perform the provider search.
            var provider = await _dbContext.ProviderProperties.Where(x =>
                x.Id == id
                ).AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (provider is null)
            {
                return null;
            }

            // Convert the entities to a models.
            var result = _mapper.Map<ProviderPropertyModel>(provider);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a provider by id"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a provider " +
                "by id",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderPropertyModel> UpdateAsync(
        ProviderPropertyModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(ProviderPropertyModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ProviderPropertyEntity>(
                provider
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderPropertyModel)} " +
                    "model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(ProviderPropertyModel),
                nameof(OrangeDbContext)
                );

            // Start tracking the entity.
            _dbContext.ProviderProperties.Attach(entity);

            // Mark the entity as modified so EFCORE will update it.
            _dbContext.Entry(entity).State = EntityState.Modified;

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
                nameof(ProviderPropertyModel)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ProviderPropertyModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderPropertyModel)} " +
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
                "Failed to update a provider property!"
                );

            // ProviderProperty better context.
            throw new RepositoryException(
                message: $"The repository failed to update a provider " +
                "property!",
                innerException: ex
                );
        }
    }

    #endregion
}
