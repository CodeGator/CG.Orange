
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class maps properties for the <see cref="SettingFileCountEntity"/>
/// entity type.
/// </summary>
internal class SettingFileCountEntityMap : EntityMapBase<SettingFileCountEntity>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingFileCountEntityMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public SettingFileCountEntityMap(
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
        EntityTypeBuilder<SettingFileCountEntity> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "SettingFileCounts",
            "Orange"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Count)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.CreatedOnUtc)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.CreatedOnUtc
        },
        $"IX_SettingFileCounts"
        ).IsUnique();
    }

    #endregion
}
