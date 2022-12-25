
namespace CG.Orange.Host.ViewModels;

/// <summary>
/// This class represents a request to upload a JSON file.
/// </summary>
public class FileUploadVM
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the file.
    /// </summary>
    [Required]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// This property contains the contents of the file.
    /// </summary>
    [Required]
    public byte[] FileBytes { get; set; } = null!;

    /// <summary>
    /// This property contains the application name for the JSON settings.
    /// </summary>
    [Required]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// This property contains the optional environment name for the JSON settings.
    /// </summary>
    public string? EnvironmentName { get; set; }

    #endregion
}
