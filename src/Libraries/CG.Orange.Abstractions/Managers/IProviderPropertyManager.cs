
namespace CG.Orange.Managers;

/// <summary>
/// This interface represents an object that manages operations related to
/// <see cref="ProviderPropertyModel"/> objects.
/// </summary>
public interface IProviderPropertyManager
{
    /// <summary>
    /// This method indicates whether there are any <see cref="ProviderPropertyModel"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="ProviderPropertyModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method indicates whether there are any <see cref="ProviderPropertyModel"/> objects
    /// in the underlying storage that are associated with the given provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="ProviderPropertyModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<bool> AnyByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="ProviderPropertyModel"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="ProviderPropertyModel"/> objects in the underlying storage.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<int> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="ProviderPropertyModel"/> objects in the 
    /// underlying storage that are associated with the given provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="ProviderPropertyModel"/> objects in the underlying storage.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<int> CountByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="ProviderPropertyModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="providerProperty">The model to create in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="ProviderPropertyModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<ProviderPropertyModel> CreateAsync(
        ProviderPropertyModel providerProperty,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method deletes an existing <see cref="ProviderPropertyModel"/> object from the 
    /// underlying storage.
    /// </summary>
    /// <param name="providerProperty">The model to delete from the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task DeleteAsync(
        ProviderPropertyModel providerProperty,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for all the <see cref="ProviderPropertyModel"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="ProviderPropertyModel"/> objects.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<ProviderPropertyModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for all the <see cref="ProviderPropertyModel"/> objects
    /// that belong to the given provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="ProviderPropertyModel"/> objects.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<ProviderPropertyModel>> FindByProviderAsync(
        ProviderModel provider,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a matching <see cref="ProviderPropertyModel"/> object using the
    /// given identifier.
    /// </summary>
    /// <param name="id">The identifier to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="ProviderPropertyModel"/> 
    /// objects, if a match was found, of <c>NULL</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<ProviderPropertyModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method updates an existing <see cref="ProviderPropertyModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="providerProperty">The model to update in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly updated
    /// <see cref="ProviderPropertyModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<ProviderPropertyModel> UpdateAsync(
        ProviderPropertyModel providerProperty,
        string userName,
        CancellationToken cancellationToken = default
        );
}
