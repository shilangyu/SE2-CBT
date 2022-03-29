using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CbtBackend.Models;
using CbtBackend.Services;

namespace CbtBackend.Controllers;

[ApiController]
[Route("user")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase {
    public const string TokenExpireHeader = "X-Expires-After";

    private readonly ILogger<AuthenticationController> logger;
    private readonly IUserService userService;

    public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger) {
        this.logger = logger;
        this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery(Name = "email")] string email, [FromQuery(Name = "password")] string password) {
        logger.LogDebug("authenticating user with data [email = {email}, password = {password}]", email, password);

        try {
            var token = await userService.Authenticate(new UserAuthenticationRequest() {
                Email = email,
                Password = password
            });

            // append token expiration date to header
            Response.Headers.Add(TokenExpireHeader, token.TokenExpiration.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));

            // return token as string
            return Ok(token.Token);
        } catch (AuthenticationCredentialsException) {
            return BadRequest(new { message = "invalid username or password" });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout() {
        // This is here just to adhere to the spec. This is a no-op.
        return Ok();
    }
}
