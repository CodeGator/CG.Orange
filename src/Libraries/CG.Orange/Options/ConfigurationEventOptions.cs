
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings related to seeding <see cref="ConfigurationEventModel"/>
/// objects.
/// </summary>
internal class ConfigurationEventOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a list of <see cref="ConfigurationEventModel"/> objects 
    /// to seed.
    /// </summary>
    [Required]
    public List<ConfigurationEventModel> ConfigurationEvents { get; set; } = null!;

    #endregion
}
