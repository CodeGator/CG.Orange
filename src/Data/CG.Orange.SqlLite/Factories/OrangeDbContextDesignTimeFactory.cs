
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

        // Set the connection string.
        optionsBuilder.UseSqlite(connectionString);

        // Create the and return the data-context.
        var dataContext = new OrangeDbContext(optionsBuilder.Options);

        // Return the results.
        return dataContext;
    }

    #endregion
}
