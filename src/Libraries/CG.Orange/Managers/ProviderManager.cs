
namespace CG.Orange.Managers;

internal class ProviderManager : IProviderManager
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
    internal protected readonly IProviderRepository _providerRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IProviderManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderManager"/>
    /// class.
    /// </summary>
    /// <param name="cryptographer">The cryptographer to use with this
    /// manager.</param>
    /// <param name="providerRepository">The provider repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ProviderManager(
        ICryptographer cryptographer,
        IProviderRepository providerRepository,
        ILogger<IProviderManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(cryptographer, nameof(cryptographer))
            .ThrowIfNull(providerRepository, nameof(providerRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _cryptographer = cryptographer;
        _providerRepository = providerRepository;
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
                nameof(IProviderRepository.AnyAsync)
                );

            // Perform the search.
            return await _providerRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for providers!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for providers!",
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
                nameof(IProviderRepository.CountAsync)
                );

            // Perform the search.
            return await _providerRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count providers!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count providers!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderModel> CreateAsync(
        ProviderModel provider,
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
                "Cloning the incoming provider"
                );

            // If we modify the properties of the incoming model then,
            //   from the caller's perspective, we're creating unwanted
            //   side-affects. For that reason, we'll copy it here.
            var copy = provider.QuickClone();

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderModel)
                );

            // Ensure the stats are correct.
            copy.CreatedOnUtc = DateTime.UtcNow;
            copy.CreatedBy = userName;
            copy.LastUpdatedBy = null;
            copy.LastUpdatedOnUtc = null;

            // Are there any properties with values?
            if (copy.Properties.Any(x => 
                !string.IsNullOrEmpty(x.Value)
                ))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Encrypting {count} properties for provider: {name}",
                    copy.Properties.Count(),
                    copy.Name
                    );

                // Loop through the properties with values.
                foreach (var property in copy.Properties.Where(x => 
                    !string.IsNullOrEmpty(x.Value)
                    )) 
                {
                    // Encrypt the value, at rest.
                    property.Value = await _cryptographer.AesEncryptAsync(
                        property.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.CreateAsync)
                );

            // Perform the operation.
            var newProvider = await _providerRepository.CreateAsync(
                copy,
                cancellationToken
                ).ConfigureAwait(false);

            // Are there properties with values?
            if (newProvider.Properties.Any(x => 
                !string.IsNullOrEmpty(x.Value)
                ))
            {
                // Loop through the properties with values.
                foreach (var property in provider.Properties.Where(x =>
                    string.IsNullOrEmpty(x.Value)
                    ))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting property for provider"
                        );

                    // Decrypt the value
                    property.Value = await _cryptographer.AesDecryptAsync(
                        property.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Return the result.
            return newProvider;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new provider!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task DeleteAsync(
        ProviderModel provider,
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
                "Cloning the incoming provider"
                );

            // If we modify the properties of the incoming model then,
            //   from the caller's perspective, we're creating unwanted
            //   side-affects. For that reason, we'll copy it here.
            var copy = provider.QuickClone();

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderModel)
                );

            // Ensure the stats are correct.
            copy.LastUpdatedOnUtc = DateTime.UtcNow;
            copy.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.DeleteAsync)
                );

            // Perform the operation.
            await _providerRepository.DeleteAsync(
                copy,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a provider!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderModel> DisableAsync(
        ProviderModel provider,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Can we take a shortcut?
            if (provider.IsDisabled)
            {
                return provider; // Nothing to do.
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderModel)
                );

            // Ensure the stats are correct.
            provider.LastUpdatedOnUtc = DateTime.UtcNow;
            provider.LastUpdatedBy = userName;

            // The repository doesn't update the associated properties, so
            //   we don't need to encrypt them here.

            // Disable the provider.
            provider.IsDisabled = true;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _providerRepository.UpdateAsync(
                provider,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to disable a provider!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to disable a provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderModel> EnableAsync(
        ProviderModel provider,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Can we take a shortcut?
            if (!provider.IsDisabled)
            {
                return provider; // Nothing to do.
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderModel)
                );

            // Ensure the stats are correct.
            provider.LastUpdatedOnUtc = DateTime.UtcNow;
            provider.LastUpdatedBy = userName;

            // The repository doesn't update the associated properties, so
            //   we don't need to encrypt them here.

            // Enable the provider.
            provider.IsDisabled = false;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _providerRepository.UpdateAsync(
                provider,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to enable a provider!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to enable a provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<IEnumerable<ProviderModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.FindAllAsync)
                );

            // Perform the operation.
            var data = await _providerRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // We must create the secondary list here because the loop(s)
            //   below create temporary objects inside the enumeration, so,
            //   the work we do to decrypt the property values gets lost
            //   unless we manually copy the modified objects to another list.
            var result = new List<ProviderModel>(); 

            // Loop through ALL the providers.
            foreach(var provider in data)
            {
                // Does this provider have properties?
                if (provider.Properties.Any())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting {count} properties for provider: {name}",
                        provider.Properties.Count(),
                        provider.Name
                        );

                    // Loop through the properties with values.
                    foreach (var property in provider.Properties.Where(x =>
                        !string.IsNullOrEmpty(x.Value)
                        ))
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Decrypting property for provider"
                            );

                        // Decrypt the value.
                        property.Value = await _cryptographer.AesDecryptAsync(
                            property.Value,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }

                // Add to the list - with or without properties.
                result.Add(provider);
            }            

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
            throw new ManagerException(
                message: $"The manager failed to search for providers!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderModel?> FindByTagAndTypeAsync(
        string tag,
        ProviderType? providerType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(tag, nameof(tag));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.FindByTagAndTypeAsync)
                );

            // Perform the operation.
            var result = await _providerRepository.FindByTagAndTypeAsync(
                tag,
                providerType,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (result is null)
            {
                return null;
            }

            // Are there any properties?
            if (result.Properties.Any())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting {count} properties for provider: {name}",
                    result.Properties.Count(),
                    result.Name
                    );

                // Loop through the properties with values.
                foreach (var property in result.Properties.Where(x =>
                    !string.IsNullOrEmpty(x.Value)
                    ))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting property for provider"
                        );

                    // Decrypt the value.
                    property.Value = await _cryptographer.AesDecryptAsync(
                        property.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
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
                "Failed to search for providers by tag and type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for providers by " +
                "tag and type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
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
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.FindByIdAsync)
                );

            // Perform the operation.
            var result = await _providerRepository.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (result is null)
            {
                return null;
            }

            // Are there any properties?
            if (result.Properties.Any())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting {count} properties for provider: {name}",
                    result.Properties.Count(),
                    result.Name
                    );

                // Loop through the properties with values.
                foreach (var property in result.Properties.Where(x => 
                    !string.IsNullOrEmpty(x.Value)
                    ))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting property for provider"
                        );

                    // Decrypt the value.
                    property.Value = await _cryptographer.AesDecryptAsync(
                        property.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
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
                "Failed to search for providers by id!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for providers by " +
                "id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc />
    public virtual async Task<ProviderModel> UpdateAsync(
        ProviderModel provider,
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
                "Cloning the incoming provider"
                );

            // If we modify the properties of the incoming model then,
            //   from the caller's perspective, we're creating unwanted
            //   side-affects. For that reason, we'll copy the model
            //   before we manipulate it.
            var copy = provider.QuickClone();

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderModel)
                );

            // Ensure the stats are correct.
            copy.LastUpdatedOnUtc = DateTime.UtcNow;
            copy.LastUpdatedBy = userName;

            // The repository doesn't update the associated properties, so
            //   we don't need to encrypt them here.

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderRepository.UpdateAsync)
                );

            // Perform the operation.
            var changedProvider = await _providerRepository.UpdateAsync(
                copy,
                cancellationToken
                ).ConfigureAwait(false);

            // Are there properties with values?
            if (changedProvider.Properties.Any(x => 
                string.IsNullOrEmpty(x.Value)
                ))
            {
                // Loop through the properties with values.
                foreach (var property in changedProvider.Properties.Where(x => 
                    !string.IsNullOrEmpty(x.Value)
                    ))
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting property for provider"
                        );

                    // Decrypt the value
                    property.Value = await _cryptographer.AesDecryptAsync(
                        property.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Return the result.
            return changedProvider;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a provider!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a provider!",
                innerException: ex
                );
        }
    }

    #endregion
}
