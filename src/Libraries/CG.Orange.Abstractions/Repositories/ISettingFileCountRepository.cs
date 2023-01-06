
namespace CG.Orange.Repositories;

/// <summary>
/// This interface represents an object that manages the storage and retrieval
/// of <see cref="SettingFileCountModel"/> objects.
/// </summary>
public interface ISettingFileCountRepository
{
    /// <summary>
    /// This method indicates whether there are any <see cref="SettingFileCountModel"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="SettingFileCountModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="SettingFileCountModel"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="SettingFileCountModel"/> objects in the underlying storage.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<int> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="SettingFileCountModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="settingFileCount">The model to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="SettingFileCountModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<SettingFileCountModel> CreateAsync(
        SettingFileCountModel settingFileCount,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for all the <see cref="SettingFileCountModel"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="SettingFileCountModel"/> objects.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<IEnumerable<SettingFileCountModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        );
}
