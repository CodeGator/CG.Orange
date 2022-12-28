
namespace CG.Orange.Seeding.Options;

/// <summary>
/// This class contains configuration options related to setting file seeding.
/// </summary>
public class SettingFileOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the application name for the settings.
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the environment name for the settings.
    /// </summary>
    [MaxLength(32)]
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the JSON for the settings.
    /// </summary>
    [Required]
    public string Json { get; set; } = null!;

    #endregion
}
