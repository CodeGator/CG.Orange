
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
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a new <see cref="SecretClient"/>
    /// instance.</returns>
    public virtual Task<SecretClient> CreateAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(provider, nameof(provider));

        // Look for the provider property.
        var keyVaultUriProperty = provider.Properties.FirstOrDefault(x => 
            string.Compare(x.Key, "keyvaulturi", StringComparison.InvariantCultureIgnoreCase) == 0
            );
        
        // Did we fail?
        if (keyVaultUriProperty is null)
        {
            // Panic!!
            throw new KeyNotFoundException(
                $"The provider property 'KeyVaultUri; was not found for provider: {provider.Id}!"
                );
        }

        // Look for the optional provider property.
        var retryDelayProperty = provider.Properties.FirstOrDefault(x =>
            string.Compare(x.Key, "retrydelay", StringComparison.InvariantCultureIgnoreCase) == 0
            );

        // Look for the optional provider property.
        var maxDelayProperty = provider.Properties.FirstOrDefault(x =>
            string.Compare(x.Key, "maxdelay", StringComparison.InvariantCultureIgnoreCase) == 0
            );

        // Look for the optional provider property.
        var maxRetriesProperty = provider.Properties.FirstOrDefault(x =>
            string.Compare(x.Key, "maxretries", StringComparison.InvariantCultureIgnoreCase) == 0
            );

        // Create the Azure client.
        var client = new SecretClient(
            new Uri(keyVaultUriProperty.Value),
            new DefaultAzureCredential(),
            new SecretClientOptions()
            {
                Retry =
                {
                    Delay = retryDelayProperty is not null ? TimeSpan.Parse(retryDelayProperty.Value) : TimeSpan.FromSeconds(2),
                    MaxDelay = maxDelayProperty is not null ? TimeSpan.Parse(maxDelayProperty.Value) : TimeSpan.FromSeconds(9),
                    MaxRetries = maxRetriesProperty is not null ? int.Parse(maxRetriesProperty.Value) : 3,
                    Mode = RetryMode.Exponential
                }
            });

        // Return the results.
        return Task.FromResult(client);
    }

    #endregion
}
