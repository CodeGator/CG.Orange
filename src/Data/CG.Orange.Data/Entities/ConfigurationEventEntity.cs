
using System.ComponentModel.DataAnnotations;

namespace CG.Orange.Data.Entities;

/// <summary>
/// This class is an entity that represents the publication of a configuration, 
/// to an Orange client.
/// </summary>
public class ConfigurationEventEntity
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the associated application name.
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the associated environment name.
    /// </summary>
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the amount of time to deliver the configuration.
    /// </summary>
    public TimeSpan ElapsedTime { get; set; }

    /// <summary>
    /// This property contains the client identifier from the caller.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// This property contains the host name from the caller.
    /// </summary>
    public string? HostName { get; set; }

    /// <summary>
    /// This property contains the UTC date/time when the event took place
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    #endregion
}
