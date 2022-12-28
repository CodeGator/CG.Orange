
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static class WebApplicationBuilderExtensions003
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the data-access layer for the <see cref="CG.Orange"/>
    /// project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section to use for the 
    /// operation. Defaults to <c>DAL</c>.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddDataAccessLayer(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "DAL",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring DAL options from the {section} section",
            sectionName
            );

        // Configure the DAL options.
        webApplicationBuilder.Services.ConfigureOptions<OrangeDalOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var dalOptions
            );
                
        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Fetching the EFCORE migration assembly name"
            );

        // We include migrations in this assembly.
        var migrationAssembly = Assembly.GetExecutingAssembly()
            .GetName().Name;

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Building a safe strategy name"
            );

        // Get the storage strategy name.
        var safeStrategyName = dalOptions.Strategy.ToLower().Trim();

        // Sanity check the strategy parameters.
        switch (safeStrategyName)
        {
            case "sqlserver":
                var connectionString = webApplicationBuilder.Configuration.GetSection($"DAL:{safeStrategyName}")["ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Panic!!
                    throw new ArgumentException(
                        message: $"The connection string at {sectionName}:{safeStrategyName}:ConnectionString, " +
                        "in the appSettings, json file is required for migrations, " +
                        "but is currently missing, or empty!"
                        );
                }
                break;
            case "sqlite":
                connectionString = webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Panic!!
                    throw new ArgumentException(
                        message: $"The connection string at {sectionName}:{safeStrategyName}:ConnectionString, " +
                        "in the appSettings, json file is required for migrations, " +
                        "but is currently missing, or empty!"
                        );
                }
                break;
            default:
                var databaseName = webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["DatabaseName"];
                if (string.IsNullOrEmpty(databaseName))
                {
                    // Panic!!
                    throw new ArgumentException(
                        message: $"The database name at {sectionName}:{safeStrategyName}:DatabaseName, " +
                        "in the appSettings, json file is required for migrations, " +
                        "but is currently missing, or empty!"
                        );
                }
                break;
        }

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the DAL {ctx} data-context using strategy: {name}",
            nameof(OrangeDbContext),
            safeStrategyName
            );

        // Wire up the EFCORE data context factory.
        webApplicationBuilder.Services.AddDbContextFactory<OrangeDbContext>(options => 
        {
            // Wire up the correct data storage strategy.
            switch (safeStrategyName)
            {
                case "sqlserver":
                    options.UseSqlServer(
                        webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["ConnectionString"]
                            ?? "Server=localhost;Database=CG.Orange;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationAssembly);
                            sqlOptions.UseQuerySplittingBehavior(
                                QuerySplittingBehavior.SplitQuery
                                );
                        });
                    break;
                case "sqlite":
                    options.UseSqlite(
                        webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["ConnectionString"]
                            ?? "Data Source=orange.db",
                        sqliteOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationAssembly);
                            sqlOptions.UseQuerySplittingBehavior(
                                QuerySplittingBehavior.SplitQuery
                                );
                        });
                    break;
                default:
                    options.UseInMemoryDatabase(
                        webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["DatabaseName"] 
                            ?? "orange" 
                        );
                    break;
            }

            // Are we running in a development environment?
            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                // Tell the world what we are about to do.
                bootstrapLogger?.LogDebug(
                    "Enabling sensitive logging for EFCORE since we're in a development environment"
                    );

                // Enable sensitive logging.
                options.EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }
        });

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the DAL auto-mapper"
            );

        // Wire up the auto-mapper.
        webApplicationBuilder.Services.AddAutoMapper(cfg =>
        {
            // Wire up the conversion maps.
            cfg.CreateMap<CG.Orange.EfCore.Entities.SettingFile, SettingFile>().ReverseMap();
        });

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the DAL repositories"
            );

        // Wire up the repositories.
        webApplicationBuilder.Services.AddScoped<ISettingFileRepository, SettingFileRepository>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
