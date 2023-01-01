
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class maps properties for the <see cref="SettingFileEntity"/>
/// entity type.
/// </summary>
internal class SettingFileEntityMap : AuditedEntityMapBase<SettingFileEntity>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileEntityMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public SettingFileEntityMap(
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
        EntityTypeBuilder<SettingFileEntity> builder
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
            .HasMaxLength(Globals.Models.SettingFiles.ApplicationNameLength)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.EnvironmentName)
            .HasMaxLength(Globals.Models.SettingFiles.EnvironmentNameLength);

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
        $"IX_SettingFiles1"
        ).IsUnique();
    }

    #endregion
}
