
using CG.Orange.Host.Hubs;

namespace CG.Orange.Host.Pages.Admin.Settings;

/// <summary>
/// This class is the code-behind for the <see cref="Setting"/> page.
/// </summary>
public partial class Setting
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a reference to breadcrumbs for the view.
    /// </summary>
    protected List<BreadcrumbItem> _crumbs = new();

    /// <summary>
    /// This field contains the model for the page.
    /// </summary>
    internal protected SettingFileModel? _model;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the setting.
    /// </summary>
    [Parameter]
    public int SettingId { get; set; }

    /// <summary>
    /// This property contains the SignalR hub for the page.
    /// </summary>
    [Inject]
    protected SignalRHub Hub { get; set; } = null!;

    /// <summary>
    /// This property contains the dialog service for the page.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for the page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the HTTP context accessor.
    /// </summary>
    [Inject]
    protected IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    /// <summary>
    /// This property contains the navigation manager for the page.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// This property contains the setting file manager for the page.
    /// </summary>
    [Inject]
    protected ISettingFileManager SettingFileManager { get; set; } = null!;

    /// <summary>
    /// This property contains the provider manager for the page.
    /// </summary>
    [Inject]
    protected IProviderManager ProviderManager { get; set; } = null!;

    /// <summary>
    /// This property contains the name of the current user, or the word
    /// 'anonymous' if nobody is currently authenticated.
    /// </summary>
    protected string UserName => HttpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<Setting> Logger { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called to initialize the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating bread crumbs."
                );

            // Create the bread crumbs for the page.
            _crumbs = new()
            {
                new BreadcrumbItem("Home", href: "/"),
                new BreadcrumbItem("Admin", href: "/admin", disabled: true),
                new BreadcrumbItem("Settings", href: "/admin/settings"),
                new BreadcrumbItem("Setting", href: $"/admin/settings/setting/{SettingId}")
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the setting file.
            _model = await SettingFileManager.FindByIdAsync(SettingId);

            // Log what we are about to do.
            Logger.LogDebug(
                "Initializing the page."
                );

            // Give the base class a chance.
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when the user submits the form.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnValidSubmitAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Validating the model reference."
                );

            // Is the model missing?
            if (_model is null)
            {
                return;
            }

            // Is the JSON invalid?
            if (!await OnValidateJsonAsync())
            {
                return;
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the change to the database."
                );

            // Save the changes.
            _model = await SettingFileManager.UpdateAsync(
                _model,
                UserName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Signaling a change through SignalR."
                );

            // Notify any watchers that the configuration changed.
            await Hub.OnChangedSettingAsync(
                _model.ApplicationName,
                _model.EnvironmentName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"Changes were saved",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method is called to validate the JSON for the model.
    /// </summary>
    /// <returns>A task to perform the operation that returns <c>true</c>
    /// if the JSON is valid, <c>false</c> otherwise.</returns>
    protected async Task<bool> OnValidateJsonAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Validating the model reference."
                );

            // Is the model missing?
            if (_model is null)
            {
                return false;
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Loading JSON into a memory stream."
                );

            // Wrap the JSON for the configuration builder.
            using var memStream = new MemoryStream(
                Encoding.UTF8.GetBytes(_model.Json)
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Parsing and validating the JSON."
                );

            // By loading the data into a configuration builder,
            //   we're effectively parsing and validating the JSON
            //   without having to write all that parsing/validation
            //   logic ourselves.
            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(memStream);
            var cfg = builder.Build();

            // Get a list of all providers.
            var providers = (await ProviderManager.FindAllAsync());

            // Now that we know the JSON is well formed, let's walk
            //   down the tree and validate any replacement tokens.
            foreach (var kvp in cfg.AsEnumerable())
            {
                // Can we parse a token?
                if (ReplacementToken.TryParse(
                    kvp.Value ?? "",
                    out var secretTag,
                    out var cacheTag,
                    out var altKey
                    ))
                {
                    // The secret tag is required.
                    if (string.IsNullOrEmpty(secretTag))
                    {
                        // Prompt the user.
                        await DialogService.ShowMessageBox(
                            title: "Orange",
                            markupMessage: new MarkupString("The JSON contains a key: " +
                            $"'{kvp.Key}' with replacement token where the <b>secret tag</b> is " +
                            $"missing or empty.")
                            );

                        return false; // Json not valid.
                    }

                    // Sanity check the secret tag.
                    if (!providers.Any(x => x.Tag == secretTag))
                    {
                        // Prompt the user.
                        await DialogService.ShowMessageBox(
                            title: "Orange",
                            markupMessage: new MarkupString("The JSON contains a key: " +
                            $"'{kvp.Key}' with replacement token where the <b>secret tag</b> doesn't " +
                            $"match a current provider's tag.")
                            );

                        return false; // Json not valid.
                    }

                    // Is the provider disabled?
                    if (providers.Any(x => x.Tag == secretTag &&
                        x.IsDisabled
                        ))
                    {
                        // Prompt the user.
                        await DialogService.ShowMessageBox(
                            title: "Orange",
                            markupMessage: new MarkupString("The JSON contains a key: " +
                            $"'{kvp.Key}' with replacement token where the <b>secret tag<b/> points " +
                            $"to a disabled provider.")
                            );

                        return false; // Json not valid.
                    }

                    // Is there a cache tag?
                    if (!string.IsNullOrEmpty(cacheTag))
                    {
                        // Sanity check the cache tag.
                        if (!providers.Any(x => x.Tag == cacheTag))
                        {
                            // Prompt the user.
                            await DialogService.ShowMessageBox(
                                title: "Orange",
                                markupMessage: new MarkupString("The JSON contains a key: " +
                                $"'{kvp.Key}' with replacement token where the <b>cache tag</b> doesn't " +
                                $"match a current provider's tag.")
                                );

                            return false; // Json not valid.
                        }

                        // Is the provider disabled?
                        if (providers.Any(x => x.Tag == cacheTag &&
                            x.IsDisabled
                            ))
                        {
                            // Prompt the user.
                            await DialogService.ShowMessageBox(
                                title: "Orange",
                                markupMessage: new MarkupString("The JSON contains a key: " +
                                $"'{kvp.Key}' with replacement token where the <b>cache tag</b> points " +
                                $"to a disabled provider.")
                                );

                            return false; // Json not valid.
                        }
                    }
                }
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"The JSON is valid",
                Severity.Normal,
                options => options.CloseAfterNavigation = true
                );

            // Json is valid.
            return true;
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"The JSON is invalid! " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );

            // Json is invalid.
            return false;
        }
    }

    #endregion
}
