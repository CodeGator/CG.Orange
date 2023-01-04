
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class maps properties for the <see cref="ProviderEntity"/>
/// entity type.
/// </summary>
internal class ProviderEntityMap : AuditedEntityMapBase<ProviderEntity>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderEntityMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ProviderEntityMap(
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
    /// This method configures the <see cref="Entities.ProviderEntity"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.ProviderEntity> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "Providers",
            "Orange"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.ProviderType)
            .HasMaxLength(Globals.Models.Providers.ProviderTypeLength)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.Name)
            .HasMaxLength(Globals.Models.Providers.NameLength)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.Description)
            .HasMaxLength(Globals.Models.Providers.DescriptionLength);

        // Setup the column.
        builder.Property(e => e.Tag)
            .HasMaxLength(Globals.Models.Providers.TagLength)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.ProcessorType)
            .HasMaxLength(Globals.Models.Providers.ProcessorTypeLength)
            .IsUnicode(false)
            .IsRequired();

        // Setup the property.
        builder.Property(e => e.IsDisabled)
            .IsRequired();

        // Setup the conversion.
        _modelBuilder.Entity<ProviderEntity>()
            .Property(e => e.ProviderType)
            .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<ProviderType>(e)
                );

        // Setup the relationship.
        _modelBuilder.Entity<ProviderPropertyEntity>()
            .HasOne(e => e.Provider)
            .WithMany(e => e.Properties)
            .HasForeignKey(e => e.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Tag
        },
        $"IX_Provider_Tags"
        ).IsUnique();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Name,
            e.IsDisabled,
            e.ProviderType,
            e.ProcessorType
        },
        $"IX_Providers"
        );
    }

    #endregion
}
