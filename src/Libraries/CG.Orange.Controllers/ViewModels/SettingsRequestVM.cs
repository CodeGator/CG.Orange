
namespace CG.Orange.Host.ViewModels;

/// <summary>
/// This class represents a request for configuration settings from a 
/// client provider.
/// </summary>
public class SettingsRequestVM
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the application for the settings.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Application { get; set; } = null!;

    /// <summary>
    /// This property contains the optional environment for the settings.
    /// </summary>
    [MaxLength(64)]
    public string? Environment { get; set; }

    #endregion
}

