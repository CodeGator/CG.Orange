
namespace CG.Orange.Directors;

/// <summary>
/// This interface represent an object that produces statistics suitable for
/// display on a dashboard.
/// </summary>
public interface IDashboardDirector
{
    /// <summary>
    /// This method returns data for a setting file count chart.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns data for the
    /// given chart.</returns>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task<SettingFileCountChartModel> GetSettingFileCountDataAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method returns data for a configuration event chart.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns data for the
    /// given chart.</returns>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task<ConfigurationEventChartModel> GetConfigurationEventDataAsync(
        CancellationToken cancellationToken = default
        );
}
