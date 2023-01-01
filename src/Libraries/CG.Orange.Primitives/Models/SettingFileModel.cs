
namespace CG.Orange.Models;

/// <summary>
/// This class represents a JSON settings file.
/// </summary>
public class SettingFileModel : AuditedModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the settings.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the application name for the settings.
    /// </summary>
    [Required]
    [MaxLength(Globals.Models.SettingFiles.ApplicationNameLength)]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the optional environment name for the 
    /// settings.
    /// </summary>
    [MaxLength(Globals.Models.SettingFiles.EnvironmentNameLength)]
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the JSON for the settings.
    /// </summary>
    [Required]
    public string Json { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the settings are disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method returns a hashcode for the object.
    /// </summary>
    /// <returns>A hashcode for the object.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    // *******************************************************************

    /// <summary>
    /// This method determines equality for the object.
    /// </summary>
    /// <param name="obj">The object to use for the operation.</param>
    /// <returns><c>true</c> if the given object is equal to the current
    /// instance, <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj)
    {
        // If the object is NULL they aren't equal.
        if (obj is null)
        {
            return false;
        }

        // If the types don't match they aren't equal.
        if (obj is not SettingFileModel)
        {
            return false;
        }

        // Identity is determined by the Id property.
        return (obj as SettingFileModel)?.Id == Id;
    }

    #endregion
}
