﻿
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class represents a base map for entity types derived from <see cref="AuditedEntityBase"/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal abstract class AuditedEntityMapBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditedEntityBase
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the model builder for this map.
    /// </summary>
    internal protected readonly ModelBuilder _modelBuilder;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AuditedEntityMapBase{TEntity}"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public AuditedEntityMapBase(
        ModelBuilder modelBuilder
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(modelBuilder, nameof(modelBuilder));

        // Save the reference(s).
        _modelBuilder = modelBuilder;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method configures the <typeparamref name="TEntity"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public virtual void Configure(
        EntityTypeBuilder<TEntity> builder
        )
    {
        // Setup the property.
        builder.Property(e => e.CreatedOnUtc)
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();

        // Setup the property.
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(Globals.Models.AuditedModelBases.CreatedByLength)
            .IsUnicode(false)
            .IsRequired();

        // Setup the property.
        builder.Property(e => e.LastUpdatedBy)
            .HasMaxLength(Globals.Models.AuditedModelBases.LastUpdatedByLength)
            .IsUnicode(false);

        // Setup the property.
        builder.Property(e => e.LastUpdatedOnUtc);

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.CreatedOnUtc,
            e.CreatedBy,
            e.LastUpdatedBy,
            e.LastUpdatedOnUtc
        },
        $"IX_{typeof(TEntity).Name}_Stats"
        );
    }

    #endregion
}
