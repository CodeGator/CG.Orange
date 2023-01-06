
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IStatisticsDirector"/>
/// interface.
/// </summary>
internal class StatisticsDirector : IStatisticsDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IStatisticsDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="StatisticsDirector"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this director.</param>
    public StatisticsDirector(
        ILogger<IStatisticsDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    #endregion
}
