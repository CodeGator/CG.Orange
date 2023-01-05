
namespace CG.Orange.Utilities;

/// <summary>
/// This class utility contains logic to parse replacement tokens.
/// </summary>
public static class ReplacementToken
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method determines whether the value contains a replacement 
    /// token, or not.
    /// </summary>
    /// <param name="value">The value to by tested.</param>
    /// <returns><c>true</c> if the value contains a replacement token, 
    /// <c>false</c> otherwise.</returns>
    public static bool HasToken(
        string value
        )
    {
        // Sanity check the value.
        if (!string.IsNullOrEmpty(value))
        {
            // Does the value contain start AND end characters?
            if (value.Length > 2 && value.StartsWith("##") && value.EndsWith("##"))
            {
                // Return the results.
                return true; 
            }
        }

        // Return the results.
        return false; 
    }

    // *******************************************************************

    /// <summary>
    /// This method parses a replacement token.
    /// </summary>
    /// <param name="token">The replacement token to be parsed.</param>
    /// <param name="secretTag">The parsed secret tag (if any).</param>
    /// <param name="cacheTag">The parsed cache tag (if any).</param>
    /// <param name="altKey">The parsed alternate key (if any).</param>
    /// <returns><c>true</c> if the value was parsed, <c>false</c> 
    /// otherwise.</returns>
    public static bool TryParse(
        string token,
        out string? secretTag,
        out string? cacheTag,
        out string? altKey
        )
    {
        // Make the compiler happy.
        secretTag = null;
        cacheTag = null;
        altKey = null;

        // Sanity check the token.
        if (!HasToken(token))
        {
            // Return the results.
            return false;
        }

        // Trim the token delimiters.
        var value = token.TrimStart('#').TrimEnd('#');

        // Try to split the value.
        var parts = value.Split(':');

        // Save the secret tag.
        secretTag = parts[0];

        // Is there a cache tag?
        if (parts.Length == 2)
        {
            // Save the cache tag.
            cacheTag = parts[1];
        }

        // Is there an alternate key?
        if (parts.Length == 3)
        {
            // Save the alt key.
            altKey = parts[2];
        }

        // Return the results.
        return true;
    }

    #endregion
}
