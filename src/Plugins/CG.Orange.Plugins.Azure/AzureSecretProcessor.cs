
namespace CG.Orange.Plugins.Azure;

/// <summary>
/// This class is an Azure implementation of the <see cref="ISecretProcessor"/>
/// interface.
/// </summary>
internal class AzureSecretProcessor : ISecretProcessor
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the Azure secret client factory for this processor. 
    /// </summary>
    internal protected readonly AzureSecretClientFactory _secretClientFactory = null!;

    /// <summary>
    /// This field contains the logger for this processor. 
    /// </summary>
    internal protected readonly ILogger<AzureSecretProcessor> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AzureSecretProcessor"/>
    /// class.
    /// </summary>
    /// <param name="secretClientFactory">The Azure secret client factory 
    /// to use with this processor.</param>
    /// <param name="logger">The logger to use with this processor.</param>
    public AzureSecretProcessor(
        AzureSecretClientFactory secretClientFactory,
        ILogger<AzureSecretProcessor> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(secretClientFactory, nameof(secretClientFactory))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _secretClientFactory = secretClientFactory;   
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<string> GetSecretAsync(
        string secretKey, 
        CancellationToken cancellationToken = default
        )
    {/*
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(secretKey, nameof(secretKey));

        try
        {
            // Get the plain-text value of the secret.
            var secretResponse = await _secretClient.GetSecretAsync(
                secretKey
                );

            // Return the results.
            return secretResponse.Value.Value;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to retrieve secret: {key} from Azure!",
                secretKey
                );

            // Provider better context for the error.
            throw new ProviderException(
                innerException: ex,
                message: $"Failed to retrieve secret {secretKey} from Azure!"
                );
        }*/

        throw new NotImplementedException();
    }

    #endregion
}
