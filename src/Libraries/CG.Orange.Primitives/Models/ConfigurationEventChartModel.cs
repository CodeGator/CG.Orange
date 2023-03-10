
namespace CG.Orange.Models;

/// <summary>
/// This class is a model that represents the configuration event chart.
/// </summary>
public class ConfigurationEventChartModel
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a collection of chart labels.
    /// </summary>
    public List<string> Labels { get; set; } = new();

    /// <summary>
    /// This property contains a <see cref="ChartSeriesModel"/> objects.
    /// </summary>
    public List<ChartSeriesModel> Series { get; set; } = new();

    #endregion
}

