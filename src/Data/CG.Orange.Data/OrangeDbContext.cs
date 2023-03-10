
namespace CG.Orange.Data;

/// <summary>
/// This class is a data-context for the <see cref="CG.Orange"/>
/// microservice.
/// </summary>
public class OrangeDbContext : DbContext
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the list of configuration events.
    /// </summary>
    public virtual DbSet<ConfigurationEventEntity> ConfigurationEvents { get; set; } = null!;

    /// <summary>
    /// This property contains the list of providers.
    /// </summary>
    public virtual DbSet<ProviderEntity> Providers { get; set; } = null!;

    /// <summary>
    /// This property contains the list of provider properties.
    /// </summary>
    public virtual DbSet<ProviderPropertyEntity> ProviderProperties { get; set; } = null!;

    /// <summary>
    /// This property contains the list of setting files.
    /// </summary>
    public virtual DbSet<SettingFileEntity> SettingFiles { get; set; } = null!;

    /// <summary>
    /// This property contains the list of setting files counts.
    /// </summary>
    public virtual DbSet<SettingFileCountEntity> SettingFileCounts { get; set; } = null!;

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
        modelBuilder.ApplyConfiguration(new ConfigurationEventEntityMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new SettingFileEntityMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new SettingFileCountEntityMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new ProviderEntityMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new ProviderPropertyEntityMap(modelBuilder));

        // Give the base class a chance.
        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
