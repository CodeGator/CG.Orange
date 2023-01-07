
namespace CG.Orange.Options;

/// <summary>
/// This class contains configuration settings for dashboard operations.
/// </summary>
public class DashboardOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the maximum amount of history to keep at 
    /// any point in time.
    /// </summary>
    public TimeSpan MaxHistory { get; set; } = TimeSpan.FromDays(7);

    #endregion
}
