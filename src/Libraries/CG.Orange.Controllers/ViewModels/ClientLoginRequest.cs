
namespace CG.Orange.Host.ViewModels;

/// <summary>
/// This class represents a client login request.
/// </summary>
public class ClientLoginRequest
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the client identifier for the request.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// This property contains the client secret for the request.
    /// </summary>
    [MaxLength(64)]
    public string? ClientSecret { get; set; }

    #endregion
}

