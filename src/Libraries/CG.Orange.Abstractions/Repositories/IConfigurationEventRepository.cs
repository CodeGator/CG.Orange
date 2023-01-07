
namespace CG.Orange.Repositories;

/// <summary>
/// This interface represents an object that manages the storage and retrieval
/// of <see cref="ConfigurationEventModel"/> objects.
/// </summary>
public interface IConfigurationEventRepository
{
    /// <summary>
    /// This method indicates whether there are any <see cref="ConfigurationEventModel"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="ConfigurationEventModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="ConfigurationEventModel"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="ConfigurationEventModel"/> objects in the underlying storage.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<int> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="ConfigurationEventModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="configurationEvent">The model to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="ConfigurationEventModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ConfigurationEventModel> CreateAsync(
        ConfigurationEventModel configurationEvent,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method removes any statistics that are older than the specified
    /// maximum history value.
    /// </summary>
    /// <param name="maxHistory">The maximum amount of history to keep at any moment in
    /// time.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task DeleteOlderThanAsync(
        TimeSpan maxHistory,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for all the <see cref="ConfigurationEventModel"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="ConfigurationEventModel"/> objects.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<IEnumerable<ConfigurationEventModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        );
}
