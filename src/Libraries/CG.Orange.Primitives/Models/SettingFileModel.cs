
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
}
