namespace CG.Orange.Host.Options;

/// <summary>
/// This class contains configuration settings for the warmup service.
/// </summary>
internal class WarmupServiceOptions
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

    #endregion
}
