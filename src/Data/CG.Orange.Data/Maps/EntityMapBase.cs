﻿
namespace CG.Orange.Data.Maps;

/// <summary>
/// This class represents a base map for entity types.
/// </summary>
/// <typeparam name="TEntity">The type of associated concrete entity</typeparam>
internal abstract class EntityMapBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
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
    /// This constructor creates a new instance of the <see cref="EntityMapBase{TEntity}"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public EntityMapBase(
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
    public abstract void Configure(
        EntityTypeBuilder<TEntity> builder
        );

    #endregion
}
