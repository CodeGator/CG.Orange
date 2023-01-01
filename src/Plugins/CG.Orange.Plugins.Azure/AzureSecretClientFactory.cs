
namespace CG.Orange.Plugins.Azure;

/// <summary>
/// This class is a factory for creating <see cref="SecretClient"/> instances.
/// </summary>
internal class AzureSecretClientFactory
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method creates a new <see cref="SecretClient"/> instance.
    /// </summary>
    /// <param name="keyVaultUri">The Azure key vault URI to use for the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a new <see cref="SecretClient"/>
    /// instance.</returns>
    public virtual Task<SecretClient> CreateAsync(
        string keyVaultUri,
        CancellationToken cancellationToken = default
        )
    {
        // Create the Azure client.
        var client = new SecretClient(
            new Uri(keyVaultUri),
            new DefaultAzureCredential(),
            new SecretClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(9),
                    MaxRetries = 3,
                    Mode = RetryMode.Exponential
                }
            });

        // Return the results.
        return Task.FromResult(client);
    }

    #endregion
}
