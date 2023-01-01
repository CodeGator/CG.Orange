
namespace CG.Orange.Data.Entities;

/// <summary>
/// This class represents a property entity for a provider entity.
/// </summary>
public class ProviderPropertyEntity : AuditedEntityBase
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
    public virtual ProviderEntity Provider { get; set; } = null!;

    /// <summary>
    /// This property contains the key for the property.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// This property contains the value for the property.
    /// </summary>
    public string Value { get; set; } = null!;

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
        return Id.GetHashCode() ^ ProviderId.GetHashCode();
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
        if (obj is not ProviderPropertyEntity)
        {
            return false;
        }

        // Identity is determined by the Id and ProviderId properties.
        return (obj as ProviderPropertyEntity)?.Id == Id &&
            (obj as ProviderPropertyEntity)?.ProviderId == ProviderId;
    }

    #endregion
}
