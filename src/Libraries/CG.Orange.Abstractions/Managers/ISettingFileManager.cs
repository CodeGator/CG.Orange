
namespace CG.Orange.Managers;

/// <summary>
/// This interface represents an object that manages operations related to
/// <see cref="SettingFileModel"/> objects.
/// </summary>
public interface ISettingFileManager
{
    /// <summary>
    /// This method indicates whether there are any <see cref="SettingFileModel"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="SettingFileModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method indicates whether there are any <see cref="SettingFileModel"/> objects
    /// in the underlying storage that match the given application and environment
    /// names.
    /// </summary>
    /// <param name="applicationName">The application name to use for the operation.</param>
    /// <param name="environmentName">The environment name to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="SettingFileModel"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        string applicationName,
        string? environmentName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="SettingFileModel"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="SettingFileModel"/> objects in the underlying storage.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<int> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="SettingFileModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="settingFile">The model to create in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="SettingFileModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<SettingFileModel> CreateAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method deletes an existing <see cref="SettingFileModel"/> object from the 
    /// underlying storage.
    /// </summary>
    /// <param name="settingFile">The model to delete from the underlying storage.</param>
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
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method disabled the setting file.
    /// </summary>
    /// <param name="provider">The model to disable.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the modified <see cref="ProviderModel"/>
    /// object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<SettingFileModel> DisableAsync(
        SettingFileModel provider,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method enables the setting file.
    /// </summary>
    /// <param name="provider">The model to enable.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the modified <see cref="SettingFileModel"/>
    /// object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<SettingFileModel> EnableAsync(
        SettingFileModel provider,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for all the <see cref="SettingFileModel"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="SettingFileModel"/> objects.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<SettingFileModel>> FindAllAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a matching <see cref="SettingFileModel"/> object using the
    /// given application and environment names.
    /// </summary>
    /// <param name="applicationName">The application name to use for the operation.</param>
    /// <param name="environmentName">The environment name to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="SettingFileModel"/> 
    /// objects, if a match was found, of <c>NULL</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<SettingFileModel?> FindByApplicationAndEnvironmentAsync(
        string applicationName,
        string? environmentName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a matching <see cref="SettingFileModel"/> object using the
    /// given identifier.
    /// </summary>
    /// <param name="id">The identifier to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="SettingFileModel"/> 
    /// objects, if a match was found, of <c>NULL</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<SettingFileModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method updates an existing <see cref="SettingFileModel"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="settingFile">The model to update in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly updated
    /// <see cref="SettingFileModel"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<SettingFileModel> UpdateAsync(
        SettingFileModel settingFile,
        string userName,
        CancellationToken cancellationToken = default
        );
}
