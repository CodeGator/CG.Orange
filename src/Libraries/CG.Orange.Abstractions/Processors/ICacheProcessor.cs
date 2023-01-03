
namespace CG.Orange.Processors;

/// <summary>
/// This interface represents a cache processor.
/// </summary>
public interface ICacheProcessor
{
    /// <summary>
    /// This method fetches a value from a remote cache.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="key">The key to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the plain-text 
    /// value associated with the key, or NULL if no value was found.</returns>
    Task<string?> GetValueAsync(
        ProviderModel provider,
        string key,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method sets a value into a remote cache.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="key">The key to use for the operation.</param>
    /// <param name="value">The value to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    Task SetValueAsync(
        ProviderModel provider,
        string key,
        string value,
        CancellationToken cancellationToken = default
        );
}
