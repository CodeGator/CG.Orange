
namespace CG.Orange.Directors;

/// <summary>
/// This interface represents an object that performs data seeding operations.
/// </summary>
public interface ISeedDirector
{
    /// <summary>
    /// This method performs a seeding operation for <see cref="ProviderModel"/>
    /// objects, from the given configuration.
    /// </summary>
    /// <param name="providers">The collection of <see cref="ProviderModel"/> 
    /// objects to use for the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when data
    /// already exists in the associated table(s), <c>false</c> to stop the 
    /// operation whenever data is detected in the associated table(s).</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    Task SeedProvidersAsync(
        IEnumerable<ProviderModel> providers,
        bool force,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="SettingFileModel"/>
    /// objects, from the given configuration.
    /// </summary>
    /// <param name="settingFiles">The collection of <see cref="SettingFileModel"/> 
    /// objects to use for the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when data
    /// already exists in the associated table(s), <c>false</c> to stop the 
    /// operation whenever data is detected in the associated table(s).</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    Task SeedSettingFilesAsync(
        IEnumerable<SettingFileModel> settingFiles,
        bool force,
        string userName,
        CancellationToken cancellationToken = default
        );
}
