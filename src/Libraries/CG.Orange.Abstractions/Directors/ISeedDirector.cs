
namespace CG.Orange.Directors;

/// <summary>
/// This interface represents an object that performs data seeding operations.
/// </summary>
public interface ISeedDirector
{
    /// <summary>
    /// This method performs a seeding operation for <see cref="SettingFileModel"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="SettingFileModel"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedSettingFilesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );
}
