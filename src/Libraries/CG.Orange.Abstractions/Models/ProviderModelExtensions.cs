
namespace CG.Orange.Models;

/// <summary>
/// This class contains extension methods related to the <see cref="ProviderModel"/>
/// type.
/// </summary>
public static class ProviderModelExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method generates a unique (temporary) key name for the given 
    /// provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <returns>A unique key name.</returns>
    public static string UniqueKeyName(
        this ProviderModel provider
        )
    {
        // Get the property count.
        var count = provider.Properties.Count() + 1;

        // Create a temporary key name.
        var propertyName = $"NewProperty{count}";

        // While there are conflicts.
        while(provider.Properties.Any(x => x.Key == propertyName))
        {
            // Try another key.
            propertyName = $"NewProperty{++count}";
        }

        // Return the results.
        return propertyName ;
    }

    #endregion
}
