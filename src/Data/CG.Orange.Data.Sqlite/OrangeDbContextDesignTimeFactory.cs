
namespace CG.Orange.Data.Sqlite;

/// <summary>
/// This class is a SQLite design time factory for the <see cref="OrangeDbContext"/>
/// data-context type.
/// </summary>
internal class OrangeDbContextDesignTimeFactory : DesignTimeDbContextFactory<OrangeDbContext>
{
    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is overridden in order to configure the options for 
    /// a Sqlite data-context instance.
    /// </summary>
    /// <param name="optionsBuilder">The data-context options builder
    /// to use for the operation.</param>
    /// <param name="configuration">The configuration to use for the operation.</param>
    protected override void OnConfigureDataContextOptions(
        DbContextOptionsBuilder<OrangeDbContext> optionsBuilder,
        IConfiguration configuration
        )
    {
        // Get the connection string from the DAL section.
        var connectionString = configuration["DAL:SQLite:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            // Panic!!
            throw new ArgumentException(
                message: "The connection string at DAL:SQLite:ConnectionString, " +
                "in the appSettings, json file is required for migrations, " +
                "but is currently missing, or empty!"
                );
        }

        // Configure the data-context options using the connection string
        //   and our migrations assembly.
        optionsBuilder.UseSqlite(
            connectionString,
            sqliteOptions =>
            {
                sqliteOptions.MigrationsAssembly(
                    Assembly.GetExecutingAssembly().GetName().Name
                    );
            });
    }

    #endregion
}
