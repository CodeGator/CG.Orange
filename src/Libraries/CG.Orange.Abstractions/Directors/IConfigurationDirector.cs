
namespace CG.Orange.Directors;

/// <summary>
/// This interface represent an object that performs configuration
/// related operations.
/// </summary>
public interface IConfigurationDirector
{
    /// <summary>
    /// This method returns a complete configuration for the given application 
    /// and/or environment - complete with associated secret values.
    /// </summary>
    /// <param name="applicationName">The application name to use for the
    /// operation.</param>
    /// <param name="environmentName">The optional environment name to use
    /// for the operation.</param>
    /// <param name="clientId">The optional client identifier to use for the 
    /// operation.</param>
    /// <param name="remoteIpAddress">The optional remote ip address to use 
    /// for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a dictionary
    /// of key-value pairs, that comprise the configuration.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task<Dictionary<string, string>> ReadConfigurationAsync(
        string applicationName,
        string? environmentName,
        string? clientId,
        string? remoteIpAddress,
        CancellationToken cancellationToken = default
        );
}
