
namespace CG.Orange.Controllers;

/// <summary>
/// This class is a REST controller for account operations.
/// </summary>

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the HTTP client for this controller.
    /// </summary>
    internal protected readonly HttpClient _httpClient = null!;

    /// <summary>
    /// This field contains the identity options for this controller.
    /// </summary>
    internal protected readonly OrangeIdentityOptions _identityOptions;

    /// <summary>
    /// This field contains the logger for this controller.
    /// </summary>
    internal protected readonly ILogger<AccountController> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AccountController"/>
    /// </summary>
    /// <param name="httpClient">The HTTP client to use with this controller.</param>
    /// <param name="identityOptions">The identity options to use with this
    /// controller.</param>
    /// <param name="logger">The logger to use with this controller.</param>
    public AccountController(
        HttpClient httpClient,
        IOptions<OrangeIdentityOptions> identityOptions,
        ILogger<AccountController> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(httpClient, nameof(httpClient))
            .ThrowIfNull(identityOptions, nameof(identityOptions))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _httpClient = httpClient;
        _identityOptions = identityOptions.Value;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method performs a client credentials grant login operation.
    /// </summary>
    /// <param name="model">The request parameters to use for the operation.</param>
    /// <returns>A task to perform the operation that returns the results 
    /// of the action.</returns>
    [HttpPost("login/client")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> ClientLoginAsync(
        [FromBody] ClientLoginRequest model
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Starting {name} method",
                nameof(ClientLoginAsync)
                );

            // Ensure the model is valid.
            if (!ModelState.IsValid)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Returning BAD REQUEST from {name} method",
                    nameof(ClientLoginAsync)
                    );

                return BadRequest(); // Nope!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Reaching out to the identity server for client: {id}",
                model.ClientId
                );

            // Log the client in.
            var result = await _httpClient.PostAsync(
                $"{_identityOptions.Authority}/connect/token",
                new FormUrlEncodedContent(
                    new Dictionary<string, string>()
                    {
                        { "grant_type", "client_credentials" },
                        { "client_id", model.ClientId },
                        { "client_secret", model.ClientSecret }
                    })
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Validating identity server response for client: {id}",
                model.ClientId
                );

            // Did we fail?
            result.EnsureSuccessStatusCode();

            // Log what we are about to do.
            _logger.LogDebug(
                "Reading the access token for client: {id}",
                model.ClientId
                );

            // Get the access token.
            var token = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            // Return the results.
            return Ok(token);
        }
        catch (Exception ex)
        {
            // Log the error in detail.
            _logger.LogError(
                ex,
                "Failed to validate the given client credentials!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to validate the given client credentials!"
                );
        }
    }

    #endregion
}
