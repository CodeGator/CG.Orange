
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

        var connectionString = configuration["DAL:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            // Panic!!
            throw new ArgumentException(
                message: "The connection string at DAL:ConnectionString, " +
                "in the appSettings, json file is required for migrations, " +
                "but is currently missing, or empty!"
                );
        }

        // Create the options builder.
        var optionsBuilder = new DbContextOptionsBuilder<OrangeDbContext>();

        // Get the storage strategy name.
        var safeStrategyName = dalOptions.Strategy.ToLower().Trim();

        // Wire up the correct data storage strategy.
        switch (safeStrategyName)
        {
            case "sqlserver":
                optionsBuilder.UseSqlServer(
                    webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["ConnectionString"]
                        ?? "Server=localhost;Database=CG.Green;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
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
                optionsBuilder.UseInMemoryDatabase(
                    webApplicationBuilder.Configuration.GetSection($"{sectionName}:{safeStrategyName}")["DatabaseName"]
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
