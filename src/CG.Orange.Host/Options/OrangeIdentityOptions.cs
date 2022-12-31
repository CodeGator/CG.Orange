
namespace CG.Orange.Host.Options;

/// <summary>
/// This class contains configuration settings for identity operations.
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

    #endregion
}
