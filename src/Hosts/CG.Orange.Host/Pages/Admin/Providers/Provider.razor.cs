
using System.Collections;

namespace CG.Orange.Host.Pages.Admin.Providers;

/// <summary>
/// This class is the code-behind for the <see cref="Provider"/> page.
/// </summary>
public partial class Provider
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a reference to breadcrumbs for the view.
    /// </summary>
    internal protected List<BreadcrumbItem> _crumbs = new();

    /// <summary>
    /// This field contains the model for the page.
    /// </summary>
    internal protected ProviderModel? _model;

    /// <summary>
    /// This field contains a temporary property model, used to restore
    /// a property when an edit is canceled.
    /// </summary>
    internal protected ProviderPropertyModel? _tempProperty;

    /// <summary>
    /// This field contains all secret processor types.
    /// </summary>
    internal protected IEnumerable<Type> _secretProcessorTypes = null!;

    /// <summary>
    /// This field contains all cache processor types.
    /// </summary>
    internal protected IEnumerable<Type> _cacheProcessorTypes = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the provider.
    /// </summary>
    [Parameter]
    public int ProviderId { get; set; }

    /// <summary>
    /// This property contains the dialog service for this page.
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
    /// This property contains the provider manager for the page.
    /// </summary>
    [Inject]
    protected IProviderManager ProviderManager { get; set; } = null!;

    /// <summary>
    /// This property contains the provider property manager for the page.
    /// </summary>
    [Inject]
    protected IProviderPropertyManager ProviderPropertyManager { get; set; } = null!;

    /// <summary>
    /// This property contains the name of the current user, or the word
    /// 'anonymous' if nobody is currently authenticated.
    /// </summary>
    protected string UserName => HttpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<Provider> Logger { get; set; } = null!;

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
                new BreadcrumbItem("Providers", href: "/admin/providers"),
                new BreadcrumbItem("Provider", href: $"/admin/providers/provider/{ProviderId}")
            };

            // Get the collection of processor types.
            _secretProcessorTypes = AppDomain.CurrentDomain.FindConcreteTypes<ISecretProcessor>();
            _cacheProcessorTypes = AppDomain.CurrentDomain.FindConcreteTypes<ICacheProcessor>();

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the provider.
            _model = await ProviderManager.FindByIdAsync(ProviderId);

            // Log what we are about to do.
            Logger.LogDebug(
                "Initializing the page."
                );

            // Give the base class a chance.
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            // Log what happened.
            Logger.LogError(
                ex,
                "Failed to initialize the page."
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

    // *******************************************************************

    /// <summary>
    /// This method is called when the user submits the form.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnValidSubmit()
    {
        try
        {
            // Sanity check the model.
            if (_model is null)
            {
                return;
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the change to the database."
                );

            // Save the changes.
            _model = await ProviderManager.UpdateAsync(
                _model,
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
        }
        catch (Exception ex)
        {
            // Log what happened.
            Logger.LogError(
                ex,
                "Failed to submit changes."
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

    // *******************************************************************

    /// <summary>
    /// This method creates a new property.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreatePropertyAsync()
    {
        try
        {
            // Sanity check the model.
            if (_model is null)
            {
                return;
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the changes."
                );

            // Create the new property.
            var newProperty = await ProviderPropertyManager.CreateAsync(
                new ProviderPropertyModel()
                {
                    ProviderId = _model.Id,
                    Key = _model.UniqueKeyName()
                },
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

            // Get the provider.
            _model = await ProviderManager.FindByIdAsync(ProviderId);
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
        
    // *******************************************************************

    /// <summary>
    /// This method deletes the given property.
    /// </summary>
    /// <param name="providerProperty">The provider property to use for 
    /// the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeletePropertyAsync(
        ProviderPropertyModel providerProperty
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
                title: "Purple",
                markupMessage: new MarkupString("This will delete the provider " +
                $"property <b>{providerProperty.Key}</b> <br /> <br /> Are " +
                "you <i>sure</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to busy."
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the change to the database."
                );

            // Defer to the manager for the delete.
            await ProviderPropertyManager.DeleteAsync(
                providerProperty,
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

            // Get the provider.
            _model = await ProviderManager.FindByIdAsync(ProviderId);
        }
        catch (Exception ex)
        {
            // Log what happened.
            Logger.LogError(
                ex,
                "Failed to delete a property"
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

    // *******************************************************************

    /// <summary>
    /// This method is called when changes to a property are committed.
    /// </summary>
    /// <param name="element">The property to use for the operation.</param>
    protected void OnCommitProperty(object element)
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Saving changes to a property."
                );

            // Save the changes.
            var changedProperty = ProviderPropertyManager.UpdateAsync(
                (ProviderPropertyModel)element,
                UserName
                ).Result;

            // We don't need the backup any more.
            _tempProperty = null;

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

            // Get the provider.
            _model = ProviderManager.FindByIdAsync(ProviderId).Result;

            // Log what we are about to do.
            Logger.LogDebug(
                "Forcing a Blazor render."
                );

            // Re-render the page.
            StateHasChanged();
        }
        catch (Exception ex)
        {
            // Log what happened.
            Logger.LogError(
                ex,
                "Failed to save changes to a property"
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

    // *******************************************************************

    /// <summary>
    /// This method is before a property is edited.
    /// </summary>
    /// <param name="element">The property to use for the operation.</param>
    protected void OnBackupProperty(object element)
    {
        // Log what we are about to do.
        Logger.LogDebug(
            "Making a temporary backup of a property."
            );

        // Make a backup copy of the property.
        _tempProperty = (ProviderPropertyModel)element.QuickClone();
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when a property edit is rolled back.
    /// </summary>
    /// <param name="element">The property to use for the operation.</param>
    protected void OnRollbackProperty(object element)
    {
        // Did we make a backup?
        if (_tempProperty is not null)
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Rolling back changes to a property."
                );

            /// Roll back the editable properties.
            ((ProviderPropertyModel)element).Key = _tempProperty.Key;
            ((ProviderPropertyModel)element).Value = _tempProperty.Value;

            // We don't need the backup any more.
            _tempProperty = null;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when the provider type changes.
    /// </summary>
    /// <param name="elements">The elements from MudBlazor</param>
    protected void OnProviderTypeChanged(object elements)
    {
        // Log what we are about to do.
        Logger.LogDebug(
            "Recovering the selected provider types."
            );

        // Do some old school casting to get to the elements.
        var providerTypes = (elements as HashSet<ProviderType>);
        if (providerTypes is not null)
        {
            // Sanity check the model.
            if (_model is not null)
            {
                // Log what we are about to do.
                Logger.LogDebug(
                    "Setting the selected provider type."
                    );

                // Get the selected provider type.
                _model.ProviderType = providerTypes.FirstOrDefault();

                // Log what we are about to do.
                Logger.LogDebug(
                    "Setting a default processor type."
                    );

                // Assign a default processor type.
                switch (_model.ProviderType)
                {
                    case ProviderType.Secret:
                        _model.ProcessorType = _secretProcessorTypes.FirstOrDefault()?.FullName ?? "";
                        break;
                    case ProviderType.Cache:
                        _model.ProcessorType = _cacheProcessorTypes.FirstOrDefault()?.FullName ?? "";
                        break;
                }

                // Log what we are about to do.
                Logger.LogDebug(
                    "Forcing a Blazor render."
                    );

                // Re-render the page.
                StateHasChanged();
            }
        }
    }

    #endregion
}
