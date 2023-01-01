
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class maps properties for the <see cref="ProviderPropertyEntity"/>
/// entity type.
/// </summary>
internal class ProviderPropertyEntityMap : AuditedEntityMapBase<ProviderPropertyEntity>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderPropertyEntityMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ProviderPropertyEntityMap(
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
    /// This method configures the <see cref="Entities.ProviderPropertyEntity"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.ProviderPropertyEntity> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "ProviderProperties",
            "Orange"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Key)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.Value)
            .IsUnicode(false)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Key
        },
        $"IX_ProviderProperties"
        ).IsUnique();
    }

    #endregion
}
