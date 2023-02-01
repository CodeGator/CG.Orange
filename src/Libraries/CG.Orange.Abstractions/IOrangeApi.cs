
namespace CG.Orange;

/// <summary>
/// This interface represents the API for the <see cref="CG.Orange"/> microservice.
/// </summary>
public interface IOrangeApi
{
    /// <summary>
    /// This property contains an object for performing configuration related
    /// operations.
    /// </summary>
    IConfigurationDirector Configurations { get; }

    /// <summary>
    /// This property contains an object for performing provider related 
    /// operations.
    /// </summary>
    IProviderDirector Providers { get; }

    /// <summary>
    /// This property contains an object for performing data seeding operations.
    /// </summary>
    ISeedDirector Seeding { get; }

    /// <summary>
    /// This property contains an object for performing setting related 
    /// operations.
    /// </summary>
    ISettingDirector Settings { get; }

    /// <summary>
    /// This property contains an object that produces statistics suitable for
    /// display on a dashboard.
    /// </summary>
    IDashboardDirector Dashboard { get; }
}
