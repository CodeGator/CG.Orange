
using System.Net.Mail;

namespace CG.Orange.Seeding.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="ISeedDirector"/>
/// interface.
/// </summary>
internal class SeedDirector : ISeedDirector
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
    internal protected readonly ILogger<ISeedDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SeedDirector"/>
    /// class.
    /// </summary>
    /// <param name="settingFileManager">The setting file manager to use with 
    /// this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public SeedDirector(
        ISettingFileManager settingFileManager,
        ILogger<ISeedDirector> logger
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
    public virtual async Task SeedSettingFilesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding setting file options."
                );

            // Bind the options.
            var settingFileOptions = new List<SettingFileOptions>();
            configuration.GetSection("SettingFiles").Bind(settingFileOptions);
                        
            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding setting files from the configuration."
                );
                        
            // Seed the setting files from the options.
            await SeedSettingFilesAsync(
                settingFileOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to seed setting files!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed setting files!",
                innerException: ex
                );
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="SettingFileOptions"/> objects.
    /// </summary>
    /// <param name="settingFileOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedSettingFilesAsync(
        List<SettingFileOptions> settingFileOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(settingFileOptions, nameof(settingFileOptions));

        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing setting files?
                var hasExistingData = await _settingFileManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding setting files because the 'force' flag " +
                        "was not specified and there are existing setting files " +
                        "in the database.",
                        settingFileOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} setting files.",
                settingFileOptions.Count
                );

            // Loop through the options.
            foreach (var settingFileOption in settingFileOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(ISettingFileManager.CreateAsync)
                    );

                // Create the file setting.
                _ = await _settingFileManager.CreateAsync(
                    new Models.SettingFile()
                    {
                        ApplicationName = settingFileOption.ApplicationName,
                        EnvironmentName = settingFileOption.EnvironmentName,
                        Json = settingFileOption.Json,
                    },
                    userName
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to seed setting files!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed setting files!",
                innerException: ex
                );
        }
    }

    #endregion
}
