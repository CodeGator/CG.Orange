
using static CG.Orange.Globals.Models;

namespace CG.Orange.Managers;

internal class ProviderPropertyManager : IProviderPropertyManager
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
    internal protected readonly IProviderPropertyRepository _providerPropertyRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IProviderPropertyManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderPropertyManager"/>
    /// class.
    /// </summary>
    /// <param name="cryptographer">The cryptographer to use with this
    /// manager.</param>
    /// <param name="providerPropertyRepository">The providerProperty property 
    /// repository to use with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ProviderPropertyManager(
        ICryptographer cryptographer,
        IProviderPropertyRepository providerPropertyRepository,        
        ILogger<IProviderPropertyManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(cryptographer, nameof(cryptographer))
            .ThrowIfNull(providerPropertyRepository, nameof(providerPropertyRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _cryptographer = cryptographer;
        _providerPropertyRepository = providerPropertyRepository;
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
                nameof(IProviderPropertyRepository.AnyAsync)
                );

            // Perform the search.
            return await _providerPropertyRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider properties!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to search for provider properties!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<bool> AnyByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.AnyByProviderAsync)
                );

            // Perform the search.
            return await _providerPropertyRepository.AnyByProviderAsync(
                provider,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for properties by provider!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to search for properties " +
                "by provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<long> CountAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.CountAsync)
                );

            // Perform the search.
            return await _providerPropertyRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count provider properties!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to count provider properties!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<int> CountByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.CountByProviderAsync)
                );

            // Perform the search.
            return await _providerPropertyRepository.CountByProviderAsync(
                provider,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count properties by provider!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to count properties by provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderPropertyModel> CreateAsync(
        ProviderPropertyModel providerProperty,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerProperty, nameof(providerProperty))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderPropertyModel)
                );

            // Ensure the stats are correct.
            providerProperty.CreatedOnUtc = DateTime.UtcNow;
            providerProperty.CreatedBy = userName;
            providerProperty.LastUpdatedBy = null;
            providerProperty.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogDebug(
                "Encrypting property for provider"
                );

            // Encrypt the value, at rest.
            providerProperty.Value = await _cryptographer.AesEncryptAsync(
                providerProperty.Key,
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.CreateAsync)
                );

            // Perform the operation.
            return await _providerPropertyRepository.CreateAsync(
                providerProperty,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new provider property!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to create a new provider " +
                "property!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task DeleteAsync(
        ProviderPropertyModel provider,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderPropertyModel)
                );

            // Ensure the stats are correct.
            provider.LastUpdatedOnUtc = DateTime.UtcNow;
            provider.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.DeleteAsync)
                );

            // Perform the operation.
            await _providerPropertyRepository.DeleteAsync(
                provider,
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
            throw new ManagerException(
                message: $"The manager failed to delete a provider property!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<IEnumerable<ProviderPropertyModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.FindAllAsync)
                );

            // Perform the operation.
            var data = await _providerPropertyRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // We must create the secondary list here because the loop
            //   below creates temporary objects inside the enumeration
            //   process, so, the work we do to decrypt the property values
            //   gets lost unless we manually copy the results to another list.
            var result = new List<ProviderPropertyModel>();

            // Loop through the properties.
            foreach (var providerProperty in data)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting value for property: {key}",
                    providerProperty.Key
                    );

                // Decrypt the value.
                providerProperty.Value = await _cryptographer.AesDecryptAsync(
                    providerProperty.Key,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Add to the list.
                result.Add(providerProperty);
            }
            
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
            throw new ManagerException(
                message: $"The manager failed to search for provider properties!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.FindByProviderAsync)
                );

            // Perform the operation.
            var result = await _providerPropertyRepository.FindByProviderAsync(
                provider,
                cancellationToken
                ).ConfigureAwait(false);

            // Loop through the properties.
            foreach (var providerProperty in result)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting value for property: {key}",
                    providerProperty.Key
                    );

                // Decrypt the value.
                providerProperty.Value = await _cryptographer.AesDecryptAsync(
                    providerProperty.Key,
                    cancellationToken
                    ).ConfigureAwait(false);
            }

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
            throw new ManagerException(
                message: $"The manager failed to search for properties! " +
                "by provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.FindByIdAsync)
                );

            // Perform the operation.
            var result = await _providerPropertyRepository.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (result is null)
            {
                return null;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Decrypting value for property: {key}",
                result.Key
                );

            // Decrypt the value.
            result.Value = await _cryptographer.AesDecryptAsync(
                result.Key,
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
                "Failed to search for a provider property by id!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to search for a provider " +
                "property by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderPropertyModel> UpdateAsync(
        ProviderPropertyModel providerProperty,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerProperty, nameof(providerProperty))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderPropertyModel)
                );

            // Ensure the stats are correct.
            providerProperty.LastUpdatedOnUtc = DateTime.UtcNow;
            providerProperty.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogDebug(
                "Encrypting property for provider"
                );

            // Encrypt the value, at rest.
            providerProperty.Value = await _cryptographer.AesEncryptAsync(
                providerProperty.Key,
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderPropertyRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _providerPropertyRepository.UpdateAsync(
                providerProperty,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a provider!"
                );

            // ProviderProperty better context.
            throw new ManagerException(
                message: $"The manager failed to update a provider!",
                innerException: ex
                );
        }
    }

    #endregion
}
