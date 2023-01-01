
namespace CG.Orange.Models;

/// <summary>
/// This class represents a property for a provider.
/// </summary>
public class ProviderPropertyModel : AuditedModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the property.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the identifier for the associated provider.
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// This property contains the associated provider.
    /// </summary>
    public ProviderModel Provider { get; set; } = null!;

    /// <summary>
    /// This property contains the key for the property.
    /// </summary>
    [Required]
    public string Key { get; set; } = null!;

    /// <summary>
    /// This property contains the value for the property.
    /// </summary>
    [Required]
    public string Value { get; set; } = null!;

    #endregion
}
