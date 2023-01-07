
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
    /// This field contains the dashboard options for this director.
    /// </summary>
    internal protected readonly DashboardOptions? _dashboardOptions;

    /// <summary>
    /// This field contains the setting file manager for this director.
    /// </summary>
    internal protected readonly ISettingFileManager _settingFileManager = null!;

    /// <summary>
    /// This field contains the setting file count manager for this director.
    /// </summary>
    internal protected readonly ISettingFileCountManager _settingFileCountManager = null!;

    /// <summary>
    /// This field contains the configuration event manager for this director.
    /// </summary>
    internal protected readonly IConfigurationEventManager _configurationEventManager = null!;

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
    /// <param name="bllOptions">The BLL options to use with this director.</param>
    /// <param name="settingFileCountManager">The setting file count manager 
    /// to use with this director.</param>
    /// <param name="configurationEventManager">The configuration event 
    /// manager to use with this director.</param>
    /// <param name="settingFileManager">The setting file manager  to use 
    /// with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public DashboardDirector(
        IOptions<OrangeBllOptions> bllOptions,
        ISettingFileCountManager settingFileCountManager,
        IConfigurationEventManager configurationEventManager,
        ISettingFileManager settingFileManager,
        ILogger<IDashboardDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(bllOptions, nameof(bllOptions))
            .ThrowIfNull(settingFileCountManager, nameof(settingFileCountManager))
            .ThrowIfNull(configurationEventManager, nameof(configurationEventManager))
            .ThrowIfNull(settingFileManager, nameof(settingFileManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _dashboardOptions = bllOptions.Value.Dashboard;
        _settingFileCountManager = settingFileCountManager;
        _configurationEventManager = configurationEventManager;
        _settingFileManager = settingFileManager;
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
            var rawData = await _settingFileCountManager.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Create a model for the chart.
            var model = new SettingFileCountChartModel();

            // Create a series for the rawData. 
            var series = new ChartSeriesModel()
            {
                Name = "Setting Files / Day [avg]"
            };

            // Get the max history from the options.
            var maxHistory = _dashboardOptions is null
                ? TimeSpan.FromDays(7)
                : _dashboardOptions.MaxHistory;

            // Get the max/min dates.
            var maxDate = DateTime.Today.Date;
            var minDate = maxDate.Subtract(maxHistory).Date;

            // Get the number of days for the chart.
            var numberOfDays = maxDate.Subtract(minDate).Days + 1;

            // Make a place to store daily rawData.
            var dailyData = new double[numberOfDays];

            // Group the raw rawData by the date.
            var groupedRawData = rawData.GroupBy(x =>
                x.CreatedOnUtc.Date
                );

            // For days with no data.
            double prevDayData = 0;

            // Loop through the days for the chart.
            for (var day = 0; day < numberOfDays; day++)
            {
                // Get the current day.
                var curDay = minDate.AddDays(day);

                // Get the current day's raw data.
                var dailyRawData = groupedRawData.FirstOrDefault(x =>
                    x.Key == curDay
                    );

                // Are we missing data for this day?
                if (dailyRawData is null || !dailyRawData.Any())
                {
                    // Default to yesterday's value.
                    dailyData[day] = prevDayData;
                }
                else
                {
                    // Average the counts.
                    dailyData[day] = dailyRawData.Average(x => x.Count);
                    prevDayData = dailyData[day];
                }

                // Add the label for the day.
                model.Labels.Add($"{curDay:MM}/{curDay:dd}");
            }

            // We won't have stats for today, since it's still in progress,
            //   so let's manufacture a count.
            dailyData[numberOfDays - 1] = await _settingFileManager.CountAsync();

            // Add the daily data to the series.
            series.Data.AddRange(dailyData);

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

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ConfigurationEventChartModel> GetConfigurationEventDataAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Defer to the manager for the query.
            var rawData = await _configurationEventManager.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Create a model for the chart.
            var model = new ConfigurationEventChartModel();

            // Create a series for the rawData. 
            var series = new ChartSeriesModel()
            {
                Name = "Configurations / Day"
            };

            // Get the max history from the options.
            var maxHistory = _dashboardOptions is null
                ? TimeSpan.FromDays(7)
                : _dashboardOptions.MaxHistory;

            // Get the max/min dates.
            var maxDate = DateTime.Today.Date;
            var minDate = maxDate.Subtract(maxHistory).Date;

            // Get the number of days for the chart.
            var numberOfDays = maxDate.Subtract(minDate).Days + 1;  

            // Make a place to store daily rawData.
            var dailyData = new double[numberOfDays];

            // Group the raw data by the date.
            var groupedRawData = rawData.GroupBy(x => 
                x.CreatedOnUtc.Date
                );

            // Loop through the days for the chart.
            for (var day = 0; day < numberOfDays; day++)
            {
                // Get the current day.
                var curDay = minDate.AddDays(day);

                // Get the current day's raw data.
                var dailyRawData = groupedRawData.FirstOrDefault(x =>
                    x.Key == curDay
                    );

                // Are we missing data for this day?
                if (dailyRawData is null || !dailyRawData.Any())
                {
                    // Default to zero.
                    dailyData[day] = 0;
                }
                else
                {
                    // Set to the count.
                    dailyData[day] = dailyRawData.Count();
                }

                // Add the label for the day.
                model.Labels.Add($"{curDay:MM}/{curDay:dd}");
            }

            // Add the daily data to the series.
            series.Data.AddRange(dailyData);

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
                "Failed to produce data for configuration event chart!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to produce data for the configuration " +
                "event chart!",
                innerException: ex
                );
        }
    }

    #endregion
}
