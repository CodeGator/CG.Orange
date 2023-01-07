
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IDashboardDirector"/>
/// interface.
/// </summary>
internal class DashboardDirector : IDashboardDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the setting file count manager for this director.
    /// </summary>
    internal protected readonly ISettingFileCountManager _settingFileCountManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IDashboardDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DashboardDirector"/>
    /// class.
    /// </summary>
    /// <param name="settingFileCountManager">The setting file count manager 
    /// to use with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public DashboardDirector(
        ISettingFileCountManager settingFileCountManager,
        ILogger<IDashboardDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFileCountManager, nameof(settingFileCountManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _settingFileCountManager = settingFileCountManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<SettingFileCountChartModel> GetSettingFileCountDataAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Defer to the manager for the query.
            var data = await _settingFileCountManager.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Create a model for the chart.
            var model = new SettingFileCountChartModel();

            // Create a series for the data. 
            var series = new ChartSeriesModel()
            {
                Name = "Setting File Counts / day"
            };

            // Loop through the data, grouped by date.
            foreach (var day in data.GroupBy(x => x.CreatedOnUtc.Date))
            {
                // Get a count of the events.
                var count = day.Count();

                double avg = 0;

                // Any reading for this day?
                if (count > 0)
                {
                    // Loop through all the day for this day.
                    foreach (var reading in day)
                    {
                        avg += reading.Count;
                    }

                    // Average the counts for the day.
                    if (count > 0)
                    {
                        avg = avg / count;
                    }
                }

                // Add the data for the day.
                series.Data.Add(avg);

                // Add the label for the day.
                model.Labels.Add($"{day.Key:MM}/{day.Key:dd}");
            }

            // Add the series to the model.
            model.Series.Add(series);

            // Return the results.
            return model;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to produce data for the setting file count chart!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to produce data for the setting " +
                "file count chart!",
                innerException: ex
                );
        }
    }

    #endregion
}
