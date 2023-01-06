﻿
namespace CG.Orange;

/// <summary>
/// This interface represents the API for the Orange microservice.
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
    /// This property contains an object for performing statistics related
    /// operations.
    /// </summary>
    IStatisticDirector Statistics { get; }
}
