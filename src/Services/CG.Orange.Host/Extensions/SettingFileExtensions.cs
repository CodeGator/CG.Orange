namespace CG.Orange.Models;

/// <summary>
/// This class contains extension methods related to the <see cref="SettingFile"/>
/// type.
/// </summary>
public static class SettingFileExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns an environment name that is safe to use from
    /// Blazor pages.
    /// </summary>
    /// <param name="settingFile">The setting file to use for the operation.</param>
    /// <returns>A safe string.</returns>
    public static string SafeEnvironmentName(
        this SettingFile settingFile
        )
    {
        // Return a safe string.
        return string.IsNullOrEmpty(settingFile.EnvironmentName) ? 
            "N/A" : settingFile.EnvironmentName;
    }

    #endregion
}
