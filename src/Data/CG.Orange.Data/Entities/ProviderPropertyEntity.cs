
namespace CG.Orange.Data.Entities;

/// <summary>
/// This class is an entity that represents a provider property.
/// </summary>
public class ProviderPropertyEntity : AuditedEntityBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the identifier for the associated provider.
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// This property contains the associated provider.
    /// </summary>
    public virtual ProviderEntity Provider { get; set; } = null!;

    /// <summary>
    /// This property contains the key for the entity.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// This property contains the value for the entity.
    /// </summary>
    public string Value { get; set; } = null!;

    #endregion
}
