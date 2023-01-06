
namespace CG.Orange;

/// <summary>
/// This class is a default implementation of the <see cref="IOrangeApi"/>
/// interface.
/// </summary>
internal class OrangeApi : IOrangeApi
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the configuration director for this API.
    /// </summary>
    internal protected readonly IConfigurationDirector _configurationDirector = null!;

    /// <summary>
    /// This field contains the provider director for this API.
    /// </summary>
    internal protected readonly IProviderDirector _providerDirector = null!;

    /// <summary>
    /// This field contains the seeding director for this API.
    /// </summary>
    internal protected readonly ISeedDirector _seedDirector = null!;

    /// <summary>
    /// This field contains the setting director for this API.
    /// </summary>
    internal protected readonly ISettingDirector _settingDirector = null!;

    /// <summary>
    /// This field contains the statistics director for this API.
    /// </summary>
    internal protected readonly IStatisticDirector _statisticsDirector = null!;

    /// <summary>
    /// This field contains the logger for this API.
    /// </summary>
    internal protected readonly ILogger<IOrangeApi> _logger = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <inheritdoc/>
    public IConfigurationDirector Configurations => _configurationDirector;

    /// <inheritdoc/>
    public IProviderDirector Providers => _providerDirector;

    /// <inheritdoc/>
    public ISeedDirector Seeding => _seedDirector;

    /// <inheritdoc/>
    public ISettingDirector Settings => _settingDirector;

    /// <inheritdoc/>
    public IStatisticDirector Statistics => _statisticsDirector;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="OrangeApi"/>
    /// class.
    /// </summary>
    /// <param name="configurationDirector">The configuration director to
    /// use with this API.</param>
    /// <param name="providerDirector">The provider director to use with 
    /// this API.</param>
    /// <param name="seedDirector">The seed director to use with this API.</param>
    /// <param name="settingDirector">The setting director to use with 
    /// this API.</param>
    /// <param name="statisticsDirector">The statistics director to
    /// use with this API.</param>
    /// <param name="logger">The logger to use with this API.</param>
    public OrangeApi(
        IConfigurationDirector configurationDirector,
        IProviderDirector providerDirector,
        ISeedDirector seedDirector, 
        ISettingDirector settingDirector,
        IStatisticDirector statisticsDirector,
        ILogger<IOrangeApi> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(configurationDirector, nameof(configurationDirector))
            .ThrowIfNull(providerDirector, nameof(providerDirector))
            .ThrowIfNull(seedDirector, nameof(seedDirector))
            .ThrowIfNull(settingDirector, nameof(settingDirector))
            .ThrowIfNull(statisticsDirector, nameof(statisticsDirector))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _configurationDirector = configurationDirector;
        _providerDirector = providerDirector;
        _seedDirector = seedDirector;
        _settingDirector = settingDirector; 
        _statisticsDirector = statisticsDirector;
        _logger = logger;
    }

    #endregion
}
