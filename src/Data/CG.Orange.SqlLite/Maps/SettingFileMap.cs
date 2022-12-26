﻿
namespace CG.Orange.SqlLite.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.SettingFile"/>
/// entity type.
/// </summary>
internal class SettingFileMap : EntityMapBase<Entities.SettingFile>
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
    /// This method configures the <see cref="Entities.SettingFile"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.SettingFile> builder
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
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.EnvironmentName)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.IsDisabled)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.IsDisabled
        },
        $"IX_FileSettings"
        );

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.ApplicationName,
            e.EnvironmentName
        },
        $"IX_FileSettings_Application"
        ).IsUnique();
    }

    #endregion
}
