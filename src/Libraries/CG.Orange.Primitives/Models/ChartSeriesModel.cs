
namespace CG.Orange.Models;

/// <summary>
/// This class is a model that represents a series for a chart.
/// </summary>
public class ChartSeriesModel
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a name for the series.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains data for the series.
    /// </summary>
    public List<double> Data { get; set; } = new();

    #endregion
}
