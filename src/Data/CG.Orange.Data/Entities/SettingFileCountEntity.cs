
namespace CG.Orange.Data.Entities;

/// <summary>
/// This class is an entity that represents a count of the number of 
/// setting files in the system, at a given point in time.
/// </summary>
public class SettingFileCountEntity
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
    /// This property contains the count of setting files.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// This property contains the UTC date/time when entity was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    #endregion
}
