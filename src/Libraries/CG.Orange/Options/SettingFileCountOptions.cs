
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings related to seeding <see cref="SettingFileCountModel"/>
/// objects.
/// </summary>
internal class SettingFileCountOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a list of <see cref="SettingFileCountModel"/> objects 
    /// to seed.
    /// </summary>
    [Required]
    public List<SettingFileCountModel> SettingFileCounts { get; set; } = null!;

    #endregion
}
