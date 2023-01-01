
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings related to seeding <see cref="SettingFileModel"/>
/// objects.
/// </summary>
internal class SettingFileOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a list of <see cref="SettingFileModel"/> objects 
    /// to seed.
    /// </summary>
    [Required]
    public List<SettingFileModel> SettingFiles { get; set; } = null!;

    #endregion
}
