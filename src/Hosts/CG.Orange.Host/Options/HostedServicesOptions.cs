namespace CG.Orange.Host.Options;

/// <summary>
/// This class contains configuration settings for the hosted services 
/// portion of the Orange business logic layer.
/// </summary>
internal class HostedServicesOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains configuration settings for the warmup service.
    /// </summary>
    public WarmupServiceOptions? WarmupService { get; set; }

    /// <summary>
    /// This property contains configuration settings for the statistics service.
    /// </summary>
    public StatisticsServiceOptions? StatisticsService { get; set; }

    #endregion
}
