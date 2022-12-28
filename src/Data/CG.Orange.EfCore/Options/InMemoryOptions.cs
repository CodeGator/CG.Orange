
namespace CG.Orange.EfCore.Options;

/// <summary>
/// This class contains configuration settings for an in-memory connection.
/// </summary>
public class InMemoryOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the connection string for the DAL.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = null!;

    #endregion
}
