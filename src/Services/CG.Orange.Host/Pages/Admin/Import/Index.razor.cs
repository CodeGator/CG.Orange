
namespace CG.Orange.Host.Pages.Admin.Import;

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
        new BreadcrumbItem("Import", href: "/admin/import")
    };

    /// <summary>
    /// This field contains the list of files to upload.
    /// </summary>
    private List<FileUploadVM> _pendingUploads = new();

    /// <summary>
    /// This field indicates when we should show the post upload prompt.
    /// </summary>
    private bool _showPrompt;

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
    /// This property contains the name of the current user, or the word
    /// 'anonymous' if nobody is currently authenticated.
    /// </summary>
    protected string UserName => HttpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called when the user selects one or more files.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    protected async Task OnFilesChangedAsync(IBrowserFile file)
    {
        try
        {
            // Remove any visible prompt.
            _showPrompt = false;

            // Check for duplicate files.
            if (_pendingUploads.Any(x => x.FileName == file.Name))
            {
                // Prompt the user.
                await DialogService.ShowMessageBox(
                    title: "Woah!",
                    markupMessage: new MarkupString($"The file: {file.Name} " +
                    "has already been uploaded.")
                    );
                return;
            }

            // Read the bytes for the file.
            var bytes = new byte[file.Size];
            using var stream = file.OpenReadStream();
            await stream.ReadAsync(bytes, 0, bytes.Length);

            // What we're doing here is looking for files with a name like
            //   this: 'A.json', or 'A.B.json' where the A part is the
            //   application name and the B part is an environment name. 

            // Of course, even if our guesses are wrong the user can still
            //   change things before they upload the file(s).

            var applicationName = "Unknown";
            var environmentName = "";
            var parts = file.Name.Split('.');

            // Does the file name contain more than 2 periods?
            if (parts.Length > 3)
            {
                applicationName = string.Join(".", parts.Take(parts.Length - 2));
                environmentName = parts[^2];
            }

            // Does the file name contain two periods?
            if (parts.Length == 3)
            {
                applicationName = parts.First();
                environmentName = parts[^2];
            }

            // Does the file name contain at least one period?
            else if (parts.Length == 2)
            {
                applicationName = parts.First();
            }

            // Add the model to the list.
            _pendingUploads.Add(new FileUploadVM()
            {
                FileName = file.Name,
                FileBytes = bytes,
                ApplicationName = applicationName,
                EnvironmentName = environmentName,
            });
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

    /// <summary>
    /// This method is called to save the uploaded files.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task SaveUploadsAsync()
    {
        try
        {
            // Loop through the uploads.
            foreach (var upload in _pendingUploads)
            {

            }

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>{_pendingUploads.Count()}</b> {(_pendingUploads.Count() == 1 ? "file was" : "files were")} saved.",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );

            // We don't need these now.
            _pendingUploads.Clear();

            // Show the post upload prompt.
            _showPrompt = true;
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

    #endregion
}
