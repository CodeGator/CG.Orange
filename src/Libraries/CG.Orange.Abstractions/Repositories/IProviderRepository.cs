
namespace CG.Orange.Repositories;

/// <summary>
/// This interface represents an object that manages the storage and retrieval of
/// <see cref="ProviderModel"/> objects.
/// </summary>
public interface IProviderRepository
{
    /// <summary>
    /// This method indicates whether there are any <see cref="ProviderModel"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="ProviderModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="ProviderModel"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="ProviderModel"/> objects in the underlying storage.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<int> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="ProviderModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="provider">The model to create in the underlying storage.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="ProviderModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ProviderModel> CreateAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method deletes an existing <see cref="ProviderModel"/> object from the 
    /// underlying storage.
    /// </summary>
    /// <param name="provider">The model to delete from the underlying storage.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task DeleteAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for all the <see cref="ProviderModel"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="ProviderModel"/> objects.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<IEnumerable<ProviderModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a matching <see cref="ProviderModel"/> object using the
    /// given provider tag and an optional <see cref="ProviderType"/> value.
    /// </summary>
    /// <param name="tag">The provider tag to use for the operation.</param>
    /// <param name="providerType">The optional provider type to use for the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="ProviderModel"/> 
    /// objects, if a match was found, of <c>NULL</c> otherwise.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ProviderModel?> FindByTagAndTypeAsync(
        string tag,
        ProviderType? providerType,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a matching <see cref="ProviderModel"/> object using the
    /// given identifier.
    /// </summary>
    /// <param name="id">The identifier to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="ProviderModel"/> 
    /// objects, if a match was found, of <c>NULL</c> otherwise.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ProviderModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method updates an existing <see cref="ProviderModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="provider">The model to update in the underlying storage.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly updated
    /// <see cref="ProviderModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ProviderModel> UpdateAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );
}
