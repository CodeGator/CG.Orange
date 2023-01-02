
using Microsoft.AspNetCore.Mvc.TagHelpers;

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
    protected List<BreadcrumbItem> _crumbs = new();

    /// <summary>
    /// This field contains the model for the page.
    /// </summary>
    internal protected ProviderModel? _model;

    /// <summary>
    /// This field contains a temporary property model, used to restore
    /// a property when an edit is canceled.
    /// </summary>
    internal protected ProviderPropertyModel? _tempProperty;

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
                new BreadcrumbItem("Providers", href: "/admin/provider"),
                new BreadcrumbItem("Provider", href: $"/admin/providers/provider/{ProviderId}")
            };

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
        
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when changes to a property are committed.
    /// </summary>
    /// <param name="element">The property to use for the operation.</param>
    private void OnCommitProperty(object element)
    {
        try
        {
            // Save the changes.
            var changedProperty = ProviderPropertyManager.UpdateAsync(
                (ProviderPropertyModel)element,
                UserName
                ).Result;

            // We don't need the backup any more.
            _tempProperty = null;

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

            // Re-render the page.
            StateHasChanged();
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
    /// This method is before a property is edited.
    /// </summary>
    /// <param name="element">The property to use for the operation.</param>
    private void OnBackupProperty(object element)
    {
        // Make a backup copy of the property.
        _tempProperty = (ProviderPropertyModel)element.QuickClone();
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when a property edit is rolled back.
    /// </summary>
    /// <param name="element">The property to use for the operation.</param>
    private void OnRollbackProperty(object element)
    {
        // Did we make a backup?
        if (_tempProperty is not null)
        {
            /// Roll back the editable properties.
            ((ProviderPropertyModel)element).Key = _tempProperty.Key;
            ((ProviderPropertyModel)element).Value = _tempProperty.Value;

            // We don't need the backup any more.
            _tempProperty = null;
        }
    }

    #endregion
}
