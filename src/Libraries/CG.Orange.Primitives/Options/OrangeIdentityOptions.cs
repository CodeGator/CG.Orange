
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings for Orange identity layer.
/// </summary>
public class OrangeIdentityOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the URL for the identity authority.
    /// </summary>
    [Required]
    public string Authority { get; set; } = null!;

    /// <summary>
    /// This property contains the client identifier.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// This property contains the client secret.
    /// </summary>
    [Required]
    public string ClientSecret { get; set; } = null!;

    /// <summary>
    /// This property contains the cookie name.
    /// </summary>
    [Required]
    public string CookieName { get; set; } = "Orange.Identity";

    /// <summary>
    /// This property indicates when a developer bypass is in effect, which 
    /// allows access to everything without requiring credentials of any
    /// kind (only works in a development environment).
    /// </summary>
    public bool DeveloperBypass { get; set; }

    #endregion
}
