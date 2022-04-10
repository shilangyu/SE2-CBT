using CbtBackend.Attributes;
using CbtBackend.Contracts;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;
using CbtBackend.Services;

namespace CbtBackend.Controllers;

[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase {
    public const string TokenExpireHeader = "X-Expires-After";

    private readonly ILogger<UsersController> logger;
    private readonly IUserService userService;

    public UsersController(IUserService userService, ILogger<UsersController> logger) {
        this.logger = logger;
        this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Login)]
    [Throttle(5, 1)]
    public async Task<IActionResult> Login([FromQuery(Name = "email")] string email, [FromQuery(Name = "password")] string password) {
        logger.LogDebug("Authenticating user with data [email = {email}, password = {password}]", email, password);

        try {
            var token = await userService.AuthenticateUserAsync(new UserAuthenticationRequest() {
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

    [HttpPost(ApiRoutes.User.Logout)]
    public IActionResult Logout() {
        // This is here just to adhere to the spec. This is a no-op.
        return Ok();
    }

    [HttpGet(ApiRoutes.User.GetAll)]
    public async Task<IActionResult> GetAllUsers() {
        return Ok(await userService.GetAllUsersAsync());
    }

    [HttpGet(ApiRoutes.User.GetByEmail)]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email) {

        var user = await userService.GetUserByEmailAsync(email);

        if (user == null) {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut(ApiRoutes.User.UpdateByEmail)]
    public async Task<IActionResult> UpdateUser([FromRoute] string email, [FromBody] UserUpdateRequest userRequest) {
        logger.LogDebug("Updating user with data [email = {email}, password = {password}], age= {age}, gender= {gender}, banned= {banned}, userStatus= {status}",
            userRequest.Email, userRequest.Password, userRequest.Age, userRequest.Gender, userRequest.Banned, userRequest.UserStatus);

        try {
            var response = await userService.UpdateUserAsync(email, userRequest);
            return Ok(new { });

        } catch (RegistrationException e) {
            if (e.Message.Equals("User does not exist")) {
                return NotFound();
            }
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete(ApiRoutes.User.DeleteByEmail)]
    public async Task<IActionResult> DeleteUser([FromRoute] string email) {
        logger.LogDebug("Deleting user with data [email = {email}]", email);

        try {
            var response = await userService.DeleteUserAsync(email);
            return Ok(new { });

        } catch (DeleteException e) {
            if (e.Message.Equals("User does not exist")) {
                return NotFound();
            }
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost(ApiRoutes.User.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRequest) {
        logger.LogDebug("Registering user with data [email = {email}, password = {password}], age= {age}, gender= {gender}, banned= {banned}",
            userRequest.Email, userRequest.Password, userRequest.Age, userRequest.Gender, userRequest.Banned);

        try {
            var response = await userService.RegisterUserAsync(userRequest);

            return Ok(new { });
            // the lines below is the proper way to return (using Created()) but spec wants us to return status code 200, perhaps
            // we can talk with other groups and change the return status to 201. keeping the code in case it's decided to use 201.
            //var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            //var locationUri = baseUrl + "/" + ApiRoutes.User.GetByEmail.Replace("{email}", user.Email);
            //return Created(locationUri, response);
        } catch (RegistrationException e) {
            return BadRequest(new { message = e.Message });
        }
    }
}
