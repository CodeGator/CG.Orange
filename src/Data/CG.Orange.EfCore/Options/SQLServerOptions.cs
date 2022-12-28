
namespace CG.Orange.EfCore.Options;

/// <summary>
/// This class contains configuration settings for a SQLServer connection.
/// </summary>
public class SQLServerOptions
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
