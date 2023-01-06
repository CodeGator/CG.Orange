namespace CG.Orange.Host.Options;

/// <summary>
/// This class contains configuration settings for the statistics service.
/// </summary>
internal class StatisticsServiceOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates whether the service is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains an optional startup delay for the service.
    /// </summary>
    public TimeSpan? StartupDelay { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// This property contains an optional operating delay for the service.
    /// </summary>
    public TimeSpan? OperatingDelay { get; set; } = TimeSpan.FromMinutes(5);

    #endregion
}
