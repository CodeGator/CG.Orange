
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings related to data seeding.
/// </summary>
internal class SeedingOptions
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
