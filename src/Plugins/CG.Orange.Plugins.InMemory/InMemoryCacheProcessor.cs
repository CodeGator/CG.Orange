
namespace CG.Orange.Plugins.InMemory;

/// <summary>
/// This class is an Azure implementation of the <see cref="ICacheProcessor"/>
/// interface.
/// </summary>
internal class InMemoryCacheProcessor : ICacheProcessor
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this processor. 
    /// </summary>
    internal protected readonly ILogger<InMemoryCacheProcessor> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="InMemoryCacheProcessor"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this processor.</param>
    public InMemoryCacheProcessor(
        ILogger<InMemoryCacheProcessor> logger
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
