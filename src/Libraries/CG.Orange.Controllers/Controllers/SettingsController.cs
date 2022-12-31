
namespace CG.Orange.Host.Controllers;

/// <summary>
/// This class is a REST controller for <see cref="SettingFileModel"/> resources.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SettingsController : ControllerBase
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for the controller.
    /// </summary>
    private readonly ILogger<SettingsController> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingsController"/>
    /// </summary>
    /// <param name="logger">The logger to use with this controller.</param>
    public SettingsController(
        ILogger<SettingsController> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a collection of configuration settings for the
    /// given application and environment.
    /// </summary>
    /// <param name="model">The request parameters to use for the operation.</param>
    /// <returns>A task to perform the operation that returns the results 
    /// of the action.</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> PostAsync(
        [FromBody] SettingsRequestVM model
        )
    {
        try
        {
            // TODO : write the code for this.

            return Ok();
        }
        catch (Exception ex)
        {
            // Log the error in detail.
            _logger.LogError(
                ex,
                "Failed to render settings as key-value-pairs!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to render settings as key-value-pairs!"
                );
        }
    }

    #endregion
}
