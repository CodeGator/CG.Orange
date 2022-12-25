
namespace CG.Orange.Models;

/// <summary>
/// This class represents a settings JSON file.
/// </summary>
public class SettingsFile
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    public int Id { get; set; }
    public string ApplicationName { get; set; }
    public string? EnvironmentName { get; set; }
    public string OriginalFileName { get; set; }
    public int Length { get; set; }
    public string Json { get; set; }
    public bool IsDisabled { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTime? LastEditedOnUtc { get; set; }

    #endregion
}
