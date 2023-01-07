
namespace CG.Orange.Host.Pages.Home;

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
    /// This field contains the data for the setting file count chart.
    /// </summary>
    internal protected List<ChartSeries> _settingFileCounts = new();

    /// <summary>
    /// This field contains the labels for the setting file count chart.
    /// </summary>
    internal protected string[] _settingFileCountLabels = Array.Empty<string>();

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the API for the page.
    /// </summary>
    [Inject]
    protected IOrangeApi OrangeApi { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for the page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method initializes the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Get the data for the setting file count chart.
            var series = await OrangeApi.Dashboard.GetSettingFileCountDataAsync();

            // Convert the data to something MudBlazor understands.
            _settingFileCounts = series.Series.Select(x => new ChartSeries()
            {
                Data = x.Data.ToArray(),
                Name = x.Name
            }).ToList();

            // Convert the labels to something MudBlazor understands.
            _settingFileCountLabels = series.Labels.ToArray();

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

    #endregion
}
