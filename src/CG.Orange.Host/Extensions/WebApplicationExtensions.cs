
using CG.Orange.Host;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplication"/>
/// type.
/// </summary>
internal static class WebApplicationExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method registers any identity middleware required to integrate 
    /// with an ODIC identity authority.
    /// </summary>
    /// <param name="webApplication">The web application builder to 
    /// use for the operation.</param>
    /// <returns>The value of the <paramref name="webApplication"/>
    /// parameter, for chaining calls together, Fluent style</returns>
    public static WebApplication UseOrangeIdentity(
        this WebApplication webApplication
        )
    {
        webApplication.UseAuthorization();
        webApplication.UseAuthentication();

        // Return the application.
        return webApplication;
    }

    #endregion
}
