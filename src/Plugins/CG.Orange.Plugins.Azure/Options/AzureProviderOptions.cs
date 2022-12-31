
namespace CG.Orange.Plugins.Azure.Options;

/// <summary>
/// This class contains configuration settings for the azure provider.
/// </summary>
internal class AzureProviderOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the URI to the azure secret key vault.
    /// </summary>
    [Required]
    public string KeyVaultUri { get; set; } = null!;

    #endregion
}
