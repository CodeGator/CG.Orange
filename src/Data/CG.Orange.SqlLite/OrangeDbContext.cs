
namespace CG.Orange.SqlLite;

/// <summary>
/// This class is a data-context for the <see cref="CG.Orange"/>
/// microservice.
/// </summary>
internal class OrangeDbContext : DbContext
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the list of setting files.
    /// </summary>
    public virtual DbSet<Entities.SettingFile> SettingFiles { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="OrangeDbContext"/>
    /// class.
    /// </summary>
    /// <param name="options">The options to use with this data-context.</param>
    public OrangeDbContext(
        DbContextOptions<OrangeDbContext> options
        ) : base(options)
    {

    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called to create the data model.
    /// </summary>
    /// <param name="modelBuilder">The builder to use for the operation.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map the entities.
        modelBuilder.ApplyConfiguration(new SettingFileMap(modelBuilder));

        // Give the base class a chance.
        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
