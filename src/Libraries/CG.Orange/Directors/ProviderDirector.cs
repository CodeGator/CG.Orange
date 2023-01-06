
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IProviderDirector"/>
/// interface.
/// </summary>
internal class ProviderDirector : IProviderDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the provider manager for this director.
    /// </summary>
    internal protected readonly IProviderManager _providerManager = null!;

    /// <summary>
    /// This field contains the provider property manager for this director.
    /// </summary>
    internal protected readonly IProviderPropertyManager _providerPropertyManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IProviderDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <inheritdoc/>
    public IProviderPropertyManager Properties => _providerPropertyManager;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderDirector"/>
    /// class.
    /// </summary>
    /// <param name="providerManager">The provider manager to use with
    /// this director.</param>
    /// <param name="providerPropertyManager">The provider property manager 
    /// to use with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public ProviderDirector(
        IProviderManager providerManager,
        IProviderPropertyManager providerPropertyManager,
        ILogger<IProviderDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerManager, nameof(providerManager))
            .ThrowIfNull(providerPropertyManager, nameof(providerPropertyManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _providerManager = providerManager;
        _providerPropertyManager = providerPropertyManager;
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
            return await _providerManager.AnyAsync(
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
            throw new DirectorException(
                message: $"The director failed to search for providers!",
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
            return await _providerManager.CountAsync(
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
            throw new DirectorException(
                message: $"The director failed to search for providers!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
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
            // Defer to the manager.
            return await _providerManager.CreateAsync(
                provider,
                userName,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a provider!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to create a provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
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
            // Defer to the manager.
            await _providerManager.DeleteAsync(
                provider,
                userName,
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
            throw new DirectorException(
                message: $"The director failed to delete a provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
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
            // Defer to the manager.
            return await _providerManager.DisableAsync(
                provider,
                userName,
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
            throw new DirectorException(
                message: $"The director failed to disable a provider!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
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
            // Defer to the manager.
            return await _providerManager.EnableAsync(
                provider,
                userName,
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
            throw new DirectorException(
                message: $"The director failed to enable a provider!",
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
            // Defer to the manager.
            return await _providerManager.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find providers!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find providers!",
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
        Guard.Instance().ThrowIfNullOrEmpty(tag, nameof(tag));

        try
        {
            // Defer to the manager.
            return await _providerManager.FindByTagAndTypeAsync(
                tag,
                providerType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find providers by tag and type!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find providers by " +
                "tag and type!",
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
            // Defer to the manager.
            return await _providerManager.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to find a provider by id!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to find a provider by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
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
            // Defer to the manager.
            return await _providerManager.UpdateAsync(
                provider,
                userName,
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

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to update a provider!",
                innerException: ex
                );
        }
    }

    #endregion
}
