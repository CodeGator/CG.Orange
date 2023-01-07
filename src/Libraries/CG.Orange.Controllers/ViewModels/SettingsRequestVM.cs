
namespace CG.Orange.Host.ViewModels;

/// <summary>
/// This class represents a request for a configuration from the ORANGE
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

    /// <summary>
    /// This property contains the optional client identifier for the caller.
    /// </summary>
    [MaxLength(200)]
    public string? ClientId { get; set; }

    #endregion
}

