
using Microsoft.AspNetCore.Builder;

namespace CG.Orange.SqlServer.Factories;

/// <summary>
/// This class is a default implementation of the <see cref="IDesignTimeDbContextFactory{TContext}"/>
/// interface.
/// </summary>
internal class OrangeDbContextDesignTimeFactory 
    : IDesignTimeDbContextFactory<OrangeDbContext>
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method creates a new <see cref="OrangeDbContext"/> instance.
    /// </summary>
    /// <param name="args">Optional arguments.</param>
    /// <returns>A <see cref="OrangeDbContext"/> instance.</returns>
    public OrangeDbContext CreateDbContext(string[] args)
    {
        // Create the configuration.
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appSettings.json");
        var configuration = configBuilder.Build();

        // Bind the DAL options.
        var dalOptions = new OrangeDalOptions();
        configuration.GetSection("DAL").Bind(dalOptions);

        // Get the storage strategy name.
        var safeStrategyName = dalOptions.Strategy.ToLower().Trim();

        // Sanity check the strategy name.
        if (string.IsNullOrEmpty(safeStrategyName))
        {
            // Panic!!
            throw new ArgumentException(
                message: "The strategy name at DAL:Strategy, in the " +
                "appSettings, json file is required for migrations, " +
                "but is currently missing, or empty!"
                );
        }

        // Sanity check the strategy parameters.
        switch (safeStrategyName)
        {
            case "sqlserver":
                var connectionString = configuration.GetSection($"DAL:{safeStrategyName}")["ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Panic!!
                    throw new ArgumentException(
                        message: $"The connection string at DAL:{safeStrategyName}:ConnectionString, " +
                        "in the appSettings, json file is required for migrations, " +
                        "but is currently missing, or empty!"
                        );
                }
                break;
            case "sqlite":
                connectionString = configuration.GetSection($"DAL:{safeStrategyName}")["ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Panic!!
                    throw new ArgumentException(
                        message: $"The connection string at DAL:{safeStrategyName}:ConnectionString, " +
                        "in the appSettings, json file is required for migrations, " +
                        "but is currently missing, or empty!"
                        );
                }
                break;
            default:
                var databaseName = configuration.GetSection($"DAL:{safeStrategyName}")["DatabaseName"];
                if (string.IsNullOrEmpty(databaseName))
                {
                    // Panic!!
                    throw new ArgumentException(
                        message: $"The database name at DAL:{safeStrategyName}:DatabaseName, " +
                        "in the appSettings, json file is required for migrations, " +
                        "but is currently missing, or empty!"
                        );
                }
                break;
        }

        // Create the options builder.
        var optionsBuilder = new DbContextOptionsBuilder<OrangeDbContext>();

        // We include migrations in this assembly.
        var migrationAssembly = Assembly.GetExecutingAssembly()
            .GetName().Name;

        // Wire up the correct data storage strategy.
        switch (safeStrategyName)
        {
            case "sqlserver":
                optionsBuilder.UseSqlServer(
                    configuration.GetSection($"DAL:{safeStrategyName}")["ConnectionString"]
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
                optionsBuilder.UseSqlite(
                    configuration.GetSection($"DAL:{safeStrategyName}")["ConnectionString"]
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
                optionsBuilder.UseInMemoryDatabase(
                    configuration.GetSection($"DAL:{safeStrategyName}")["DatabaseName"]
                        ?? "orange"
                    );
                break;
        }        

        // Create the and return the data-context.
        var dataContext = new OrangeDbContext(optionsBuilder.Options);

        // Return the results.
        return dataContext;
    }

    #endregion
}
