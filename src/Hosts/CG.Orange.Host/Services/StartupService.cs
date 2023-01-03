
namespace CG.Orange.Host.Services;

/// <summary>
/// The class is a hosted service that warms up the website on startup.
/// </summary>
public class StartupService : IHostedService, IDisposable
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a cancellation token source for this service.
    /// </summary>
    internal protected readonly CancellationTokenSource _cts = new();

    /// <summary>
    /// This field contains the task for this service.
    /// </summary>
    internal protected Task? _executingTask;

    /// <summary>
    /// This field contains the service provider for this service.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// This field contains the logger for this service.
    /// </summary>
    internal protected readonly ILogger<StartupService> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="StartupService"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with 
    /// this service.</param>
    /// <param name="logger">The logger to use with this service.</param>
    public StartupService(
        IServiceProvider serviceProvider,
        ILogger<StartupService> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method starts the service.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    public virtual Task StartAsync(
        CancellationToken cancellationToken
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Starting the service task."
            );

        // Store the task we're executing
        _executingTask = ExecuteAsync(_cts.Token);

        // Log what we are about to do.
        _logger.LogDebug(
            "Checking to see if the task completed."
            );

        // If the task is completed then return it, this will bubble
        // cancellation and failure to the caller
        if (_executingTask.IsCompleted)
        {
            return _executingTask;
        }

        // Otherwise it's running
        return Task.CompletedTask;
    }

    // *******************************************************************

    /// <summary>
    /// This method stops the service.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    public virtual async Task StopAsync(
        CancellationToken cancellationToken
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Checking to see stop was called without start."
            );

        // Stop called without start
        if (_executingTask == null)
        {
            return;
        }

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Signaling the cancellation token source."
                );

            // Signal cancellation to the executing method
            _cts.Cancel();
        }
        finally
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Waiting for the task to finish."
                );

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(
                _executingTask, 
                Task.Delay(
                    Timeout.Infinite, 
                    cancellationToken
                    ));
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method disposes of managed resources.
    /// </summary>
    public virtual void Dispose()
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Cleaning up the token source."
            );

        // Cleanup the token source.
        _cts.Cancel();
    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method warms up the website.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected virtual async Task ExecuteAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogInformation(
                "The {svc} is running.",
                nameof(StartupService)
                );

            // Wait before we do anything.
            await Task.Delay(
                TimeSpan.FromSeconds(30),
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a DI scope"
                );

            // Create a DI scope.
            using var scope = _serviceProvider.CreateScope();

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating setting file manager"
                );

            // Create a setting file manager.
            var settingFileManager = scope.ServiceProvider.GetRequiredService<ISettingFileManager>();

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating configuration director"
                );

            // Create a configuration director.
            var configurationDirector = scope.ServiceProvider.GetRequiredService<IConfigurationDirector>();

            // Log what we are about to do.
            _logger.LogDebug(
                "Fetching all enabled setting files"
                );

            // Get the setting files.
            var settingFiles = await settingFileManager.FindAllAsync(
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
                    _ = await configurationDirector.ReadConfigurationAsync(
                        settingFile.ApplicationName,
                        settingFile.EnvironmentName,
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
                "Failed to warmup the website."
                );
        }
        finally
        {
            // Log what we are about to do.
            _logger.LogInformation(
                "The {svc} is stopped.",
                nameof(StartupService)
                );
        }
    }

    #endregion
}
