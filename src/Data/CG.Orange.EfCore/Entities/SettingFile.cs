
namespace CG.Orange.EfCore.Entities;

/// <summary>
/// This class represents a JSON setting file entity.
/// </summary>
internal class SettingFile : EntityBase
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
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the optional environment name for the 
    /// settings.
    /// </summary>
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// This property contains the JSON for the settings.
    /// </summary>
    public string Json { get; set; } = null!;

    /// <summary>
    /// This property indicates the settings is disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    #endregion
}
