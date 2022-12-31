
namespace CG.Orange.Data.SqlServer.InMemory;

/// <summary>
/// This class is an in-memory design time factory for the <see cref="OrangeDbContext"/>
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
    /// an in-memory data-context instance.
    /// </summary>
    /// <param name="optionsBuilder">The data-context options builder
    /// to use for the operation.</param>
    /// <param name="configuration">The configuration to use for the operation.</param>
    protected override void OnConfigureDataContextOptions(
        DbContextOptionsBuilder<OrangeDbContext> optionsBuilder,
        IConfiguration configuration
        )
    {
        // Get the database name from the DAL section.
        var databaseName = configuration["DAL:InMemory:DatabaseName"];
        if (string.IsNullOrEmpty(databaseName))
        {
            // Panic!!
            throw new ArgumentException(
                message: "The database name at DAL:InMemory:DatabaseName, " +
                "in the appSettings, json file is required for migrations, " +
                "but is currently missing, or empty!"
                );
        }

        // Configure the data-context options using the database name.
        optionsBuilder.UseInMemoryDatabase(
            databaseName,
            inMemeoryOptions =>
            {

            });
    }

    #endregion
}
