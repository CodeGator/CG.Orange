
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings related to seeding <see cref="ProviderModel"/>
/// objects.
/// </summary>
internal class ProviderOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a list of <see cref="ProviderModel"/> objects 
    /// to seed.
    /// </summary>
    [Required]
    public List<ProviderModel> Providers { get; set; } = null!;

    #endregion
}
