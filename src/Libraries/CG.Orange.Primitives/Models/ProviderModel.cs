﻿
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
    /// This property contains the associated properties for the provider.
    /// </summary>
    [Required]
    public List<ProviderPropertyModel> Properties { get; set; } = new();

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method returns a hashcode for the object.
    /// </summary>
    /// <returns>A hashcode for the object.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    // *******************************************************************

    /// <summary>
    /// This method determines equality for the object.
    /// </summary>
    /// <param name="obj">The object to use for the operation.</param>
    /// <returns><c>true</c> if the given object is equal to the current
    /// instance, <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj)
    {
        // If the object is NULL they aren't equal.
        if (obj is null)
        {
            return false;
        }

        // If the types don't match they aren't equal.
        if (obj is not ProviderModel)
        {
            return false;
        }

        // Identity is determined by the Id property.
        return (obj as ProviderModel)?.Id == Id;
    }

    #endregion
}
