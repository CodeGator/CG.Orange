
namespace CG.Orange.Models;

/// <summary>
/// This class represents a JSON settings setting file.
/// </summary>
public class SettingFile : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the model.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the application name for the settings.
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the optional environment name for the 
    /// settings.
    /// </summary>
    [MaxLength(32)]
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the JSON for the settings.
    /// </summary>
    [Required]
    public string Json { get; set; } = null!;

    /// <summary>
    /// This property indicates the settings is disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    #endregion
}
