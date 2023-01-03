

namespace CG.Orange.Processors;

/// <summary>
/// This interface represents a factory for creating processor instances
/// at runtime.
/// </summary>
public interface IProcessorFactory
{
    /// <summary>
    /// This method creates a new <see cref="ICacheProcessor"/> object,
    /// based on the give tag value.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the new 
    /// <see cref="ICacheProcessor"/> instance, or NULL if the operation
    /// fails to create a processor instance.</returns>
    Task<ICacheProcessor?> CreateCacheProcessorAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="ISecretProcessor"/> object,
    /// based on the give tag value.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the new 
    /// <see cref="ISecretProcessor"/> instance, or NULL if the operation
    /// fails to create a processor instance.</returns>
    Task<ISecretProcessor?> CreateSecretProcessorAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );
}
