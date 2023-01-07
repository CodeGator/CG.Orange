
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class maps properties for the <see cref="ConfigurationEventEntity"/>
/// entity type.
/// </summary>
internal class ConfigurationEventEntityMap : EntityMapBase<ConfigurationEventEntity>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ConfigurationEventEntityMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ConfigurationEventEntityMap(
        ModelBuilder modelBuilder
        ) : base(modelBuilder)
    {

    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method configures the <see cref="SettingFileEntity"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<ConfigurationEventEntity> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "ConfigurationEvents",
            "Orange"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.ApplicationName)
            .HasMaxLength(Globals.Models.SettingFiles.ApplicationNameLength)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.EnvironmentName)
            .HasMaxLength(Globals.Models.SettingFiles.EnvironmentNameLength);

        // Setup the column.
        builder.Property(e => e.ClientId)
            .HasMaxLength(Globals.Models.ConfigurationEvents.ClientIdLength);

        // Setup the column.
        builder.Property(e => e.HostName)
            .HasMaxLength(Globals.Models.ConfigurationEvents.HostNameLength);

        // Setup the column.
        builder.Property(e => e.ElapsedTime)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.CreatedOnUtc)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.ApplicationName,
            e.EnvironmentName,
            e.ClientId,
            e.HostName,
            e.ElapsedTime,
            e.CreatedOnUtc
        },
        $"IX_ConfigurationEvents"
        ).IsUnique();
    }

    #endregion
}
