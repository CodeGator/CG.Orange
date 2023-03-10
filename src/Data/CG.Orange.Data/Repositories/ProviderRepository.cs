
namespace CG.Orange.Data.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IProviderRepository"/>
/// interface.
/// </summary>
internal class ProviderRepository : IProviderRepository
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
    internal protected readonly ILogger<IProviderRepository> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContext">The EFCORE data-context to use with this 
    /// repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public ProviderRepository(
        OrangeDbContext dbContext,
        IMapper mapper,
        ILogger<IProviderRepository> logger
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
            var data = await _dbContext.Providers.AnyAsync(
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
                "Failed to search for providers!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for providers!",
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
                "Searching for providers"
                );

            // Search for any entities in the data-store.
            var data = await _dbContext.Providers.CountAsync(
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
                "Failed to count providers!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count providers!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderModel> CreateAsync(
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
                "Converting a {entity} model to an entity",
                nameof(ProviderModel)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ProviderEntity>(
                provider
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderModel)} " +
                    "model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(ProviderEntity),
                nameof(OrangeDbContext)
                );

            // Add the entity to the data-store.
            _dbContext.Providers.Attach(entity);

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
                nameof(ProviderModel)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ProviderModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderEntity)} " +
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
                "Failed to create an provider!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create an provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
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
                "looking for the tracked {entity} instance from the {ctx} data-context",
                nameof(ProviderModel),
                nameof(OrangeDbContext)
                );

            // Find the tracked entity (if any).
            var entity = await _dbContext.Providers.FindAsync(
                provider.Id,
                cancellationToken
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The provider {provider.Id} was not found!"
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "deleting an {entity} instance from the {ctx} data-context",
                nameof(ProviderEntity),
                nameof(OrangeDbContext)
                );

            // Delete from the data-store.
            _dbContext.Providers.Remove(
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
                "Failed to delete an provider!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete an provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ProviderModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for providers."
                );

            // Perform the provider search.
            var providers = await _dbContext.Providers
                .Include(x => x.Properties)
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity(s) to model(s)",
                nameof(ProviderEntity)
                );

            // Convert the entities to a models.
            var result = providers.Select(x =>
                _mapper.Map<ProviderModel>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for providers!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for " +
                "providers!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderModel?> FindByTagAndTypeAsync(
        string tag,
        ProviderType? providerType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(tag, nameof(tag));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for a provider."
                );

            // Perform the provider search.
            var provider = await _dbContext.Providers.Where(x =>
                x.Tag == tag && 
                (providerType != null ? x.ProviderType == providerType : true)
                ).Include(x => x.Properties)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (provider is null)
            {
                return null;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(ProviderEntity)
                );

            // Convert the entities to a models.
            var result = _mapper.Map<ProviderModel>(provider);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a providers by tag and type!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for providers " +
                "by tag and type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderModel?> FindByIdAsync(
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
            var provider = await _dbContext.Providers.Where(x =>
                x.Id == id
                ).Include(x => x.Properties)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (provider is null)
            {
                return null;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(ProviderEntity)
                );

            // Convert the entities to a models.
            var result = _mapper.Map<ProviderModel>(provider);

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderEntity)} " +
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
                "Failed to search for a provider by id"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a provider " +
                "by id",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderModel> UpdateAsync(
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
                "Looking for the matching entity"
                );

            // Look for the given provider.
            var entity = await _dbContext.Providers.Where(x =>
                x.Id == provider.Id
                ).Include(x => x.Properties)
                .FirstOrDefaultAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The provider: {provider.Id} was not found!"
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the entity"
                );

            // Update the editable properties.
            entity.Name = provider.Name;
            entity.Description = provider.Description;
            entity.IsDisabled = provider.IsDisabled;
            entity.LastUpdatedBy = provider.LastUpdatedBy;  
            entity.LastUpdatedOnUtc = provider.LastUpdatedOnUtc;

            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for modified properties"
                );

            // Loop through any associated properties.
            foreach (var property in entity.Properties)
            {
                // Look for a match.
                var match = provider.Properties.FirstOrDefault(x => 
                    x.Id == property.Id
                    );

                // Did we find one?
                if (match is not null)
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Modifying property entity: {id}",
                        entity.Id
                        );

                    // Update the editable properties.
                    property.Key = match.Key;
                    property.Value = match.Value;   
                    property.LastUpdatedBy = match.LastUpdatedBy;
                    property.LastUpdatedOnUtc = match.LastUpdatedOnUtc; 
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Removing property entity: {id}",
                        property.Id
                        );

                    // Remove the mismatched property.
                    entity.Properties.Remove(property);
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for added properties"
                );

            // Loop through any new properties.
            foreach (var property in provider.Properties.Where(x => x.Id == 0))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Adding new property entity: {id}",
                    property.Id
                    );

                // Add the new property.
                entity.Properties.Add(_mapper.Map<ProviderPropertyEntity>(property));
            }

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
                nameof(ProviderEntity)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ProviderModel>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderEntity)} " +
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
                "Failed to update a provider!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a provider!",
                innerException: ex
                );
        }
    }

    #endregion
}
