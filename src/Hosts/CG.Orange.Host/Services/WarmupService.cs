
namespace CG.Orange.Host.Services;

/// <summary>
/// The class is a hosted service that warms up the Orange microservice, on 
/// startup, by pre-fetching all the settings so the secrets are pulled 
/// from their remote services, and, if applicable, cached locally.
/// </summary>
internal class WarmupService : BackgroundService
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the service provider for this service.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// This field contains the hosted service options for this service.
    /// </summary>
    internal protected readonly HostedServicesOptions _hostedServiceOptions = null!;

    /// <summary>
    /// This field contains the logger for this service.
    /// </summary>
    internal protected readonly ILogger<WarmupService> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="WarmupService"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with 
    /// this service.</param>
    /// <param name="hostedServiceOptions">The hosted service options to
    /// use with this service.</param>
    /// <param name="logger">The logger to use with this service.</param>
    public WarmupService(
        IServiceProvider serviceProvider,
        IOptions<HostedServicesOptions> hostedServiceOptions,
        ILogger<WarmupService> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(hostedServiceOptions, nameof(hostedServiceOptions))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _serviceProvider = serviceProvider;
        _hostedServiceOptions = hostedServiceOptions.Value;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method performs work for the service.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task ExecuteAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Were options provided?
            if (_hostedServiceOptions.WarmupService is not null)
            {
                // Are we disabled?
                if (_hostedServiceOptions.WarmupService.IsDisabled)
                {
                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Exiting because the {svc} service is disabled in the options",
                        nameof(WarmupService)
                        );
                    return; // Nothing to do!
                }

                // Should we wait before we start?
                if (_hostedServiceOptions.WarmupService.StartupDelay is not null)
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Calculating the startup delay"
                        );

                    // Calculate the delay.
                    var delay = _hostedServiceOptions.WarmupService.StartupDelay
                        ?? TimeSpan.FromMinutes(1);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Waiting {ts} before we start the service.",
                        delay
                        );

                    // Wait before we do anything.
                    await Task.Delay(
                        delay,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Log what we are about to do.
            _logger.LogInformation(
                "The {svc} is running.",
                nameof(WarmupService)
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a DI scope"
                );

            // Create a DI scope.
            using var scope = _serviceProvider.CreateScope();

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating an Orange API"
                );

            // Create a API.
            var orangeApi = scope.ServiceProvider.GetRequiredService<IOrangeApi>();

            // Log what we are about to do.
            _logger.LogDebug(
                "Fetching all enabled setting files"
                );

            // Get the setting files.
            var settingFiles = await orangeApi.Settings.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} setting files",
                settingFiles.Count()
                );

            // Loop through the enabled setting files.
            foreach ( var settingFile in settingFiles.Where(x => !x.IsDisabled)) 
            {
                try
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Fetching settings for application: {app}, environment: {env}",
                        settingFile.ApplicationName,
                        settingFile.EnvironmentName
                        );

                    // Read the setting, which will cache any embedded secrets.
                    _ = await orangeApi.Configurations.ReadConfigurationAsync(
                        settingFile.ApplicationName,
                        settingFile.EnvironmentName,
                        "warmup",
                        "localhost",
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Log what happened.
                    _logger.LogError(
                        ex,
                        "Failed to warmup settings for application: {app}, " +
                        "environment: {env}.",
                        settingFile.ApplicationName,
                        settingFile.EnvironmentName
                        );
                }

                // Log what we are about to do.
                _logger.LogDebug(
                    "Waiting before we fetch the next settings"
                    );

                // Wait before we continue.
                await Task.Delay(
                    TimeSpan.FromSeconds(10),
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Unexpected failure in the {name} service!",
                nameof(WarmupService)
                );
        }
        finally
        {
            // Log what we are about to do.
            _logger.LogInformation(
                "The {svc} is stopped.",
                nameof(WarmupService)
                );
        }
    }

    #endregion
}
