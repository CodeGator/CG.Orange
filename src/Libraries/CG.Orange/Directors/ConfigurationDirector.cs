
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IConfigurationDirector"/>
/// interface.
/// </summary>
internal class ConfigurationDirector : IConfigurationDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the setting file manager for this director.
    /// </summary>
    internal protected readonly ISettingFileManager _settingFileManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IConfigurationDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ConfigurationDirector"/>
    /// class.
    /// </summary>
    /// <param name="settingFileManager">The setting file manager to use 
    /// with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public ConfigurationDirector(
        ISettingFileManager settingFileManager,
        ILogger<IConfigurationDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(settingFileManager, nameof(settingFileManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _settingFileManager = settingFileManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<Dictionary<string, string>> ReadConfigurationAsync(
        string applicationName, 
        string? environmentName, 
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(applicationName, nameof(applicationName));

        // Here we're building a complete configuration for the given
        //   application and/or environment - with associated secret
        //   values.

        try
        {
            // TODO : write the code for this.

            return new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a configuration!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The manager failed to search for a configuration!",
                innerException: ex
                );
        }
    }

    #endregion
}
