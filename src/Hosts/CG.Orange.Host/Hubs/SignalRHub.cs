
namespace CG.Orange.Host.Hubs;

/// <summary>
/// This class is a SignalR hub for the Orange client back channel.
/// </summary>
public class SignalRHub : Hub
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SignalRHub"/>
    /// class.
    /// </summary>
    public SignalRHub() { }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method sends a changed notification.
    /// </summary>
    /// <param name="application">The application for the setting that was 
    /// changed.</param>
    /// <param name="environment">The optional environment for the setting 
    /// that was changed.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task to perform the operation.</returns>
    public async Task OnChangedSettingAsync(
        string application,
        string? environment,
        CancellationToken cancellationToken = default
        )
    {
        // Do we have any clients to notify?
        if (Clients is not null)
        {
            // Send the message to the clients.
            await Clients.All.SendAsync(
                "ChangedSetting",
                application,
                environment,
                cancellationToken
                ).ConfigureAwait(false);
        }
    }

    #endregion
}
