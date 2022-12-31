
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.SettingFileEntity"/>
/// entity type.
/// </summary>
internal class SettingFileMap : AuditedEntityMapBase<Entities.SettingFileEntity>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public SettingFileMap(
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
    /// This method configures the <see cref="Entities.SettingFileEntity"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.SettingFileEntity> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "SettingFiles",
            "Orange"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Json)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.ApplicationName)
            .HasMaxLength(32)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.EnvironmentName)
            .HasMaxLength(32);

        // Setup the column.
        builder.Property(e => e.IsDisabled)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.IsDisabled
        },
        $"IX_SettingsFiles"
        );

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.ApplicationName,
            e.EnvironmentName
        },
        $"IX_SettingFile1"
        ).IsUnique();
    }

    #endregion
}
