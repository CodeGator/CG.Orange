
namespace CG.Orange.Models;

/// <summary>
/// This class is a model that represents the publication of a configuration, 
/// to an Orange client.
/// </summary>
public class ConfigurationEventModel
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
    /// This property contains the associated application name.
    /// </summary>
    [Required]
    [MaxLength(Globals.Models.SettingFiles.ApplicationNameLength)]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the associated environment name.
    /// </summary>
    [MaxLength(Globals.Models.SettingFiles.EnvironmentNameLength)]
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the client identifier from the caller.
    /// </summary>
    [MaxLength(Globals.Models.ConfigurationEvents.ClientIdLength)]
    public string? ClientId { get; set; }

    /// <summary>
    /// This property contains the host name from the caller.
    /// </summary>
    [MaxLength(Globals.Models.ConfigurationEvents.HostNameLength)]
    public string? HostName { get; set; }

    /// <summary>
    /// This property contains the amount of time to deliver the configuration.
    /// </summary>
    public TimeSpan ElapsedTime { get; set; }

    /// <summary>
    /// This property contains the UTC date/time when the event took place
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    #endregion
}
