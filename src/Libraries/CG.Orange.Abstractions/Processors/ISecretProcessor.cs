
namespace CG.Orange.Processors;

/// <summary>
/// This interface represents a secret processor.
/// </summary>
public interface ISecretProcessor 
{
    /// <summary>
    /// This method fetches a secret from a remote service.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="secretKey">The secret key to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the plain-text 
    /// value of the given secret, or NULL if no secret was found.</returns>
    Task<string?> GetValueAsync(
        ProviderModel provider,
        string secretKey,
        CancellationToken cancellationToken = default
        );
}
