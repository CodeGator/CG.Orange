
namespace CG.Orange.Data.Entities;

/// <summary>
/// This class is an entity that represents a provider.
/// </summary>
public class ProviderEntity : AuditedEntityBase
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
    /// This property contains the provider type for the entity.
    /// </summary>
    public ProviderType ProviderType { get; set; }

    /// <summary>
    /// This property contains the name for the entity.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the optional description for the entity.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// This property contains the tag for the entity.
    /// </summary>
    public string Tag { get; set; } = null!;

    /// <summary>
    /// This property contains the .NET type for the associated processor.
    /// </summary>
    public string ProcessorType { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the entity is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the associated properties for the entity.
    /// </summary>
    public List<ProviderPropertyEntity> Properties { get; set; } = new();

    #endregion
}
