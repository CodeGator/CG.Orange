
namespace CG.Orange.Plugins.Azure;

/// <summary>
/// This class is an Azure implementation of the <see cref="ISecretProvider"/>
/// interface.
/// </summary>
internal class AzureSecretProvider : ISecretProvider
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the Azure client for this provider. 
    /// </summary>
    internal protected readonly SecretClient _secretClient;

    /// <summary>
    /// This field contains the logger for this provider. 
    /// </summary>
    internal protected readonly ILogger<AzureSecretProvider> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AzureSecretProvider"/>
    /// class.
    /// </summary>
    public AzureSecretProvider(
        SecretClient secretClient,
        ILogger<AzureSecretProvider> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(secretClient, nameof(secretClient))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _secretClient = secretClient;   
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
    {
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
        }
    }

    #endregion
}
