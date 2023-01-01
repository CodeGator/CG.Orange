
namespace CG.Orange.Models;

/// <summary>
/// This class represents a provider.
/// </summary>
public class ProviderModel : AuditedModelBase
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
    [Required]
    [MaxLength(Globals.Models.Providers.NameLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the optional description for the provider.
    /// </summary>
    [MaxLength(Globals.Models.Providers.DescriptionLength)]
    public string? Description { get; set; }

    /// <summary>
    /// This property contains the .NET type for the associated processor.
    /// </summary>
    [Required]
    [MaxLength(Globals.Models.Providers.ProcessorTypeLength)]
    public string ProcessorType { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the provider is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the associated properties for the provider.
    /// </summary>
    [Required]
    public List<ProviderPropertyModel> Properties { get; set; } = new();

    #endregion
}
