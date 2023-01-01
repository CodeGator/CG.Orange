
namespace CG.Orange.Models;

/// <summary>
/// This class represents a base model with audit properties.
/// </summary>
public class AuditedModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the person who created the model.
    /// </summary>
    [Required]
    [MaxLength(Globals.Models.AuditedModelBases.CreatedByLength)]
    public string CreatedBy { get; set; } = "anonymous";

    /// <summary>
    /// This property contains the date/time when the model was created.
    /// </summary>
    [Required]
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This property contains the name of the last person to update the model.
    /// </summary>
    [MaxLength(Globals.Models.AuditedModelBases.LastUpdatedByLength)]
    public string? LastUpdatedBy { get; set; } = null!;

    /// <summary>
    /// This property contains the date/time when the model was last updated.
    /// </summary>
    public DateTime? LastUpdatedOnUtc { get; set; }

    #endregion
}
