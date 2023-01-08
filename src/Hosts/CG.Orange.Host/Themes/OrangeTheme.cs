
namespace CG.Orange.Host.Themes;

/// <summary>
/// This class represents the default MudBlazor UI theme.
/// </summary>
internal class OrangeTheme : DefaultTheme<OrangeTheme>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="OrangeTheme"/>
    /// class.
    /// </summary>
    public OrangeTheme()
    {
        // Create the Orange default palette
        Palette.Primary = Colors.Purple.Darken2;
        Palette.Secondary = Colors.Green.Default;
        Palette.Tertiary = Colors.Blue.Default;
        Palette.AppbarBackground = Colors.DeepOrange.Default;
    }

    #endregion
}
