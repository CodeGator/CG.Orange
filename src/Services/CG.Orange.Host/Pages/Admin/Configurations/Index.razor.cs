
namespace CG.Orange.Host.Pages.Admin.Configurations;

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
        new BreadcrumbItem("Configurations", href: "/admin/configurations")
    };

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties
        
    #endregion
}
