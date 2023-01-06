
namespace CG.Orange.Data.Entities;

/// <summary>
/// This class is an entity that represents a JSON setting file.
/// </summary>
public class SettingFileEntity : AuditedEntityBase
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
    /// This property contains the application name for the entity.
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the optional environment name for the 
    /// entity.
    /// </summary>
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the JSON for the entity.
    /// </summary>
    public string Json { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the entity is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    #endregion
}
