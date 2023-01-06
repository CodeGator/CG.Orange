namespace CG.Orange.Host.Services;

/// <summary>
/// The class is a hosted service that periodically aggregates various 
/// statistics together for the Orange microservice.
/// </summary>
internal class StatisticsService : BackgroundService
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
    internal protected readonly ILogger<StatisticsService> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="StatisticsService"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with 
    /// this service.</param>
    /// <param name="hostedServiceOptions">The hosted service options to
    /// use with this service.</param>
    /// <param name="logger">The logger to use with this service.</param>
    public StatisticsService(
        IServiceProvider serviceProvider,
        IOptions<HostedServicesOptions> hostedServiceOptions,
        ILogger<StatisticsService> logger
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
            if (_hostedServiceOptions.StatisticsService is not null)
            {
                // Are we disabled?
                if (_hostedServiceOptions.StatisticsService.IsDisabled)
                {
                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Exiting because the {svc} service is disabled in the options",
                        nameof(StatisticsService)
                        );
                    return; // Nothing to do!
                }

                // Should we wait before we start?
                if (_hostedServiceOptions.StatisticsService.StartupDelay is not null)
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Calculating the startup delay"
                        );

                    // Calculate the delay.
                    var delay = _hostedServiceOptions.StatisticsService.StartupDelay
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
                nameof(StatisticsService)
                );

            // Run until we are stopped.
            while (!cancellationToken.IsCancellationRequested)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating a DI scope"
                    );

                // Create a DI scope.
                using var scope = _serviceProvider.CreateScope();

                // TODO : write the code for this.

                // Were options provided?
                if (_hostedServiceOptions.StatisticsService is not null)
                {
                    // Should we wait before we start?
                    if (_hostedServiceOptions.StatisticsService.OperatingDelay is not null)
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Calculating the operating delay"
                            );

                        // Calculate the delay.
                        var delay = _hostedServiceOptions.StatisticsService.OperatingDelay
                            ?? TimeSpan.FromMinutes(5);

                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Waiting {ts} before we perform more work",
                            delay
                            );

                        // Wait before we do anything.
                        await Task.Delay(
                            delay,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }
            }            
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Unexpected failure in the {name} service!",
                nameof(StatisticsService)
                );
        }
        finally
        {
            // Log what we are about to do.
            _logger.LogInformation(
                "The {svc} is stopped.",
                nameof(StatisticsService)
                );
        }
    }

    #endregion
}
