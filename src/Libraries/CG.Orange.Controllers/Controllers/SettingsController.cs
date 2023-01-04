
namespace CG.Orange.Controllers;

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
    /// This field contains the configuration director for this controller.
    /// </summary>
    internal protected readonly IConfigurationDirector _configurationDirector = null!;

    /// <summary>
    /// This field contains the logger for this controller.
    /// </summary>
    internal protected readonly ILogger<SettingsController> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SettingsController"/>
    /// </summary>
    /// <param name="configurationDirector">The configuration director to
    /// use with this controller.</param>
    /// <param name="logger">The logger to use with this controller.</param>
    public SettingsController(
        IConfigurationDirector configurationDirector,
        ILogger<SettingsController> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configurationDirector, nameof(configurationDirector))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _configurationDirector = configurationDirector;
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Starting {name} method",
                nameof(PostAsync)
                );

            // Sanity check the model state.
            if (!ModelState.IsValid) 
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Returning BAD REQUEST from {name} method",
                    nameof(PostAsync)
                    );

                return BadRequest(); // Nope!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Fetching a configuration for application: {app}, environment: {env}",
                model.Application,
                model.Environment
                );

            // Read the complete configuration (with secrets).
            var result = await _configurationDirector.ReadConfigurationAsync(
                model.Application,
                model.Environment
                ).ConfigureAwait(false);

            // Return the results.
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the error in detail.
            _logger.LogError(
                ex,
                "Failed to render a configuration!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to render a configuration!"
                );
        }
    }

    #endregion
}
