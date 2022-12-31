
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplication"/>
/// type.
/// </summary>
public static class WebApplicationExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method performs startup operations for this in-memory provider.
    /// </summary>
    /// <param name="webApplication">The web application to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplication"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    public static WebApplication UseInMemoryDataAccess(
        this WebApplication webApplication
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplication, nameof(webApplication));

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Checking the application's environment for DAL startup."
            );

        // We only touch the database in a development environment.
        if (webApplication.Environment.IsDevelopment())
        {
            // Log what we are about to do.
            webApplication.Logger.LogDebug(
                "Fetching the DAL startup options from the DI container."
                );

            // Get the DAL startup options.
            var dalStartOptions = webApplication.Services.GetRequiredService<
                IOptions<DataAcessLayerOptions>
                >();

            // Should we drop the underlying database?
            if (dalStartOptions.Value.DropDatabaseOnStartup)
            {
                // Log what we didn't do.
                webApplication.Logger.LogWarning(
                    "Skipping drop and recreate of databases because " +
                    "the in-memory provider doesn't support this feature."
                    );
            }
            else
            {
                // Log what we didn't do.
                webApplication.Logger.LogWarning(
                    "Skipping drop and recreate of databases because " +
                    "the '{flag}' flag is either false, or missing.",
                    nameof(dalStartOptions.Value.DropDatabaseOnStartup)
                    );

                // Should we apply any pending migrations?
                if (dalStartOptions.Value.MigrateDatabaseOnStartup)
                {
                    // Log what we didn't do.
                    webApplication.Logger.LogWarning(
                        "Skipping migration because the in-memory provider " +
                        "doesn't support this feature."
                        );
                }
                else
                {
                    // Log what we didn't do.
                    webApplication.Logger.LogWarning(
                        "Skipping migration because the '{flag}' flag is either " +
                        "false, or missing.",
                        nameof(dalStartOptions.Value.MigrateDatabaseOnStartup)
                        );
                }
            }
        }
        else
        {
            // Log what we didn't do.
            webApplication.Logger.LogInformation(
                "Ignoring DAL startup because we aren't in a development " +
                "environment."
                );
        }

        // Return the application.
        return webApplication;
    }

    #endregion
}
