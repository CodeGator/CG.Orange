
namespace CG.Orange.Models;

/// <summary>
/// This class is a model that represents a count of the number of 
/// setting files in the system, at a given point in time.
/// </summary>
public class SettingFileCountModel
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
    /// This property contains the count of setting files.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// This property contains the name of the person who created the model.
    /// </summary>
    [Required]
    [MaxLength(Globals.Models.SettingFileCounts.CreatedByLength)]
    public string CreatedBy { get; set; } = "anonymous";

    /// <summary>
    /// This property contains the UTC date/time when model was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    #endregion
}
