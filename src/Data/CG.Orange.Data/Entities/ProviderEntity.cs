
namespace CG.Orange.Data.Entities;

/// <summary>
/// This class represents a provider entity.
/// </summary>
public class ProviderEntity : AuditedEntityBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the provider.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the provider type.
    /// </summary>
    public ProviderType ProviderType { get; set; }  

    /// <summary>
    /// This property contains the name for the provider.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the optional description for the provider.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// This property contains the .NET type for the associated processor.
    /// </summary>
    public string ProcessorType { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the provider is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the associated properties for the provider.
    /// </summary>
    public List<ProviderPropertyEntity> Properties { get; set; } = new();

    #endregion
}
