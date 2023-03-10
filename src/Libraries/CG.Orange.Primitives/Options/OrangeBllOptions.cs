
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings for the Orange business 
/// logic layer.
/// </summary>
public class OrangeBllOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains configuration settings for dashboard operations.
    /// </summary>
    public DashboardOptions? Dashboard { get; set; }

    #endregion
}
