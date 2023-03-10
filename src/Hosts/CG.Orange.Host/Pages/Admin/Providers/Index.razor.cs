
namespace CG.Orange.Host.Pages.Admin.Providers;

/// <summary>
/// This class is the code-behind for the <see cref="Index"/> page.
/// </summary>
public partial class Index
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a reference to breadcrumbs for the view.
    /// </summary>
    protected readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Admin", href: "/admin", disabled: true),
        new BreadcrumbItem("Providers", href: "/admin/providers")
    };

    /// <summary>
    /// This field contains the list of providers.
    /// </summary>
    internal protected IEnumerable<ProviderModel> _providers = null!;

    /// <summary>
    /// This field contains the list of secret processor types.
    /// </summary>
    internal protected IEnumerable<Type> _secretProcessorTypes = null!;

    /// <summary>
    /// This field contains the list of cache processor types.
    /// </summary>
    internal protected IEnumerable<Type> _cacheProcessorTypes = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

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
    /// This property contains the API for the page.
    /// </summary>
    [Inject]
    protected IOrangeApi OrangeApi { get; set; } = null!;

    /// <summary>
    /// This property contains the name of the current user, or the word
    /// 'anonymous' if nobody is currently authenticated.
    /// </summary>
    protected string UserName => HttpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<Index> Logger { get; set; } = null!;

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
                "Refreshing the page data."
                );

            // Get the list of providers.
            _providers = await OrangeApi.Providers.FindAllAsync();

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the processor types."
                );

            // Get the list of secret processor types.
            _secretProcessorTypes = AppDomain.CurrentDomain.FindConcreteTypes<
                ISecretProcessor
                >();

            // Get the list of cache processor types.
            _cacheProcessorTypes = AppDomain.CurrentDomain.FindConcreteTypes<
                ICacheProcessor
                >();

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
    /// This method disables the given provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task DisableProviderAsync(
        ProviderModel provider
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Prompting the caller."
                );

            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Orange",
                markupMessage: new MarkupString("This will disable " +
                $"the provider <b>'{provider.Name}' <br /><br /> " +
                "Are you <i>sure</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Disabling the provider."
                );

            // Defer to the manager for the operation.
            await OrangeApi.Providers.DisableAsync(
                provider,
                UserName
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

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the list of providers.
            _providers = await OrangeApi.Providers.FindAllAsync();
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
    /// This method enables the given provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task EnableProviderAsync(
        ProviderModel provider
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Enabling the provider."
                );

            // Defer to the manager for the operation.
            await OrangeApi.Providers.EnableAsync(
                provider,
                UserName
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

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the list of providers.
            _providers = await OrangeApi.Providers.FindAllAsync();
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
    /// This method deletes the given provider.
    /// </summary>
    /// <param name="provider">The provider to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        ProviderModel provider
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Prompting the caller."
                );

            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Orange",
                markupMessage: new MarkupString("This will delete " +
                $"the provider <b>'{provider.Name}'</b> <br /><br /> " +
                "Are you <i>sure</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the change to the database."
                );

            // Remove the provider.
            await OrangeApi.Providers.DeleteAsync(
                provider,
                UserName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"Provider was deleted",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the list of providers.
            _providers = await OrangeApi.Providers.FindAllAsync();
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
    /// This method creates a new provider.
    /// </summary>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateAsync(
        ProviderType providerType
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Generating a safe provider name."
                );

            // Generate a 'safe' default provider name.
            var count = 1;
            var safeName = providerType == ProviderType.Secret 
                ? $"NewSecretProvider{count}" 
                : $"NewCacheProvider{count}";
            while (_providers.Any(x => x.Name == safeName))
            {
                // Try another name.
                safeName = providerType == ProviderType.Secret
                ? $"NewSecretProvider{++count}"
                : $"NewCacheProvider{++count}";
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Generating a safe provider tag."
                );

            // Generate a 'safe' default provider tag.
            count = 1;
            var safeTag = $"NewTag{count}";
            while (_providers.Any(x => x.Tag == safeTag))
            {
                // Try another tag.
                safeTag = $"NewTag{++count}";
            }

            // Create a default processor type.
            var defaultProcessorType = (providerType == ProviderType.Secret 
                ? _secretProcessorTypes.FirstOrDefault()?.Name 
                : _cacheProcessorTypes.FirstOrDefault()?.Name) ?? "";

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the changes."
                );

            // Create the new provider.
            var newProvider = await OrangeApi.Providers.CreateAsync(
                new ProviderModel()
                {
                    Name = safeName,
                    Tag = safeTag,
                    ProcessorType = defaultProcessorType,
                    ProviderType = providerType
                },
                UserName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Navigating to the edit page."
                );

            // Edit the new provider.
            NavigationManager.NavigateTo(
                $"/admin/providers/provider/{newProvider.Id}", 
                true
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            Logger.LogError(
                ex,
                "Failed to create a new property"
                );

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

    #endregion
}
