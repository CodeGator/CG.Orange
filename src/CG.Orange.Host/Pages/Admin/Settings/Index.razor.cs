
namespace CG.Orange.Host.Pages.Admin.Settings;

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
        new BreadcrumbItem("Settings", href: "/admin/settings")
    };

    /// <summary>
    /// This field contains the list of settings.
    /// </summary>
    internal protected IEnumerable<SettingFile> _settings = null!;

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
    /// This property contains the setting file manager for the page.
    /// </summary>
    [Inject]
    protected ISettingFileManager SettingFileManager { get; set; } = null!;

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

            // Get the list of settings.
            _settings = await SettingFileManager.FindAllAsync();

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
    /// This method is called to delete a setting file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(SettingFile file)
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
                markupMessage: new MarkupString("This will delete " +
                $"the setting file for application <b>'{file.ApplicationName}'" +
                $"</b> and environment <b>'{file.SafeEnvironmentName()}' " +
                "<br /><br /> Are you <i>sure</i> you want to do that?"),
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

            // Remove the setting file.
            await SettingFileManager.DeleteAsync(
                file,
                UserName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"setting file was deleted",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the list of settings.
            _settings = await SettingFileManager.FindAllAsync();
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
    /// This method is called when the user imports a setting file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnFilesChangedAsync(IBrowserFile file)
    {
        try
        {
            // Step 1: Read and validate the file.

            // Log what we are about to do.
            Logger.LogDebug(
                "Validating incoming file."
                );

            // Get the valid json.
            var json = await ParseAndVerifyAsync(file);

            // Step 2: Make a guess about the application / environment
            //   name(s), based on the incoming file name.

            // Log what we are about to do.
            Logger.LogDebug(
                "Guessing some names."
                );

            // Get some names.
            var names = await GuessSomeNamesAsync(file.Name);

            // Step 3: Create the model.

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the change to the database."
                );

            // Save the change.
            _ = await SettingFileManager.CreateAsync(
                new SettingFile()
                {
                    Json = json,
                    ApplicationName = names.Item1,
                    EnvironmentName = names.Item2,
                },
                UserName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"File imported",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );

            // Step 4: Refresh the list of settings.

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Get the list of settings.
            _settings = await SettingFileManager.FindAllAsync();
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
    /// This method is called when the user creates a new setting file.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateAsync()
    {
        // Make an initial guess at a new file name.
        var names = await GuessSomeNamesAsync("application.json");

        // Step 3: Create the model.
        _ = await SettingFileManager.CreateAsync(
            new SettingFile()
            {
                Json = "{ }",
                ApplicationName = names.Item1,
                EnvironmentName = names.Item2,
            },
            UserName
            );

        // Log what we are about to do.
        Logger.LogDebug(
            "Showing the snackbar message."
            );

        // Tell the world what happened.
        SnackbarService.Add(
            $"File created",
            Severity.Success,
            options => options.CloseAfterNavigation = true
            );

        // Log what we are about to do.
        Logger.LogDebug(
            "Refreshing the page data."
            );

        // Get the list of settings.
        _settings = await SettingFileManager.FindAllAsync();
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method tries to guess an application and environment name based
    /// on the given file name.
    /// </summary>
    /// <param name="fileName">The name of a JSON file.</param>
    /// <returns>A task to perform the operation that returns a tuple containing 
    /// an application and environment name.</returns>
    private async Task<(string, string)> GuessSomeNamesAsync(
        string fileName
        )
    {
        // What we're doing here is looking for files with a name like
        //   this: 'A.json', or 'A.B.json' where the A part is the
        //   application name and the B part is an environment name. 

        var applicationName = "Unknown";
        var environmentName = "";
        var parts = fileName.Split('.');

        // Does the file name contain more than 2 periods?
        if (parts.Length > 3)
        {
            // We'll say the application name is everything up to the
            //   second to the last period.
            applicationName = string.Join(".", parts.Take(parts.Length - 2));

            // We'll say the environment name is everything after the 
            //   second to the last period.
            environmentName = parts[^2];
        }

        // Does the file name contain two periods?
        if (parts.Length == 3)
        {
            // We'll say the application name is whatever is before the
            //   first period.
            applicationName = parts.First();

            // We'll say the environment name is everything after the 
            //   second to the last period.
            environmentName = parts[^2];
        }

        // Does the file name contain at least one period?
        else if (parts.Length == 2)
        {
            // We'll say the application name is whatever is before the
            //   first period.
            applicationName = parts.First();
        }

        // Now that we've made an initial guess, let's ensure the
        //   resulting application name doesn't conflict with what's
        //   already in the database.

        // Loop and check for conflicts.
        var safeApplicationName = applicationName;
        var counter = 1;
        while (await SettingFileManager.AnyAsync(
            safeApplicationName,
            environmentName
            ))
        {
            safeApplicationName = $"{applicationName}{counter++}";
        }

        // Now that we have an application and environment name that 
        // is unique, return the results.

        // Return the results.
        return (safeApplicationName, environmentName);  
    }

    // *******************************************************************

    /// <summary>
    /// This file parses and verifies the structure of the JSON in the 
    /// given <see cref="IBrowserFile"/> object.
    /// </summary>
    /// <param name="file">The file to use for the operation.</param>
    /// <returns>A task to perform the operation that returns the JSON 
    /// contents of the file.</returns>
    private async Task<string> ParseAndVerifyAsync(IBrowserFile file)
    {
        // Read the bytes for the file.
        var bytes = new byte[file.Size];
        using var stream = file.OpenReadStream();
        await stream.ReadAsync(bytes, 0, bytes.Length);

        // Convert the bytes to a string.
        var json = Encoding.UTF8.GetString(bytes);

        // Wrap the stream for the configuration builder.
        using (var memStream = new MemoryStream(bytes))
        {
            // By loading the data into a configuration builder,
            //   we're effectively parsing and validating the JSON
            //   without having to write all that parsing/validation
            //   logic ourselves.
            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(memStream);
            _ = builder.Build();
        }

        // Return the JSON.
        return json;
    }

    #endregion

}
