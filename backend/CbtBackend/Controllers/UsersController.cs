using CbtBackend.Attributes;
using CbtBackend.Contracts;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Services;

namespace CbtBackend.Controllers;

[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase {
    public const string TokenExpireHeader = "X-Expires-After";

    private readonly ILogger<UsersController> logger;
    private readonly IUserService userService;
    private readonly UserManager<User> userManager;

    public UsersController(IUserService userService, UserManager<User> userManager, ILogger<UsersController> logger) {
        this.userService = userService;
        this.userManager = userManager;
        this.logger = logger;
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Login)]
    [Throttle(5, 1)]
    public async Task<IActionResult> Login([FromBody] UserAuthenticationRequest userRequest) {
        logger.LogDebug("Authenticating user with data [email = {login}, password = {password}]", userRequest.Login, userRequest.Password);

        try {
            var response = await userService.AuthenticateUserAsync(userRequest);

            // append token expiration date to header
            Response.Headers.Add(TokenExpireHeader, response.TokenExpiration.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));

            return Ok(new LoginResponseDTO(response.Token, response.User.UserStatus, response.User.Id));
        } catch (AuthenticationCredentialsException) {
            return Unauthorized(new { message = "invalid login or password" });
        }
    }

    [HttpPost(ApiRoutes.User.Logout)]
    public IActionResult Logout() {
        // This is here just to adhere to the spec. This is a no-op.
        return Ok();
    }

    [Authorize(Roles = UserRoles.UserRead)]
    [HttpGet(ApiRoutes.User.GetAll)]
    public async Task<IActionResult> GetAllUsers() {
        var users = await userService.GetAllUsersAsync();

        return Ok(users.Select(e => new UserDTO(e)).ToList());
    }

    [Authorize(Roles = UserRoles.UserRead)]
    [HttpGet(ApiRoutes.User.GetByUserId)]
    public async Task<IActionResult> GetUserByUserId([FromRoute] int userId) {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null) {
            return NotFound();
        }

        return Ok(new UserDTO(user));
    }

    [Authorize(Roles = UserRoles.UserWrite + "," + UserRoles.UserRead)]
    [HttpPut(ApiRoutes.User.UpdateByUserId)]
    public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UserUpdateRequest userRequest) {
        logger.LogDebug("Updating user with data [email = {login}, password = {password}], age= {age}, gender= {gender}, banned= {banned}, userStatus= {status}",
            userRequest.Email, userRequest.Password, userRequest.Age, userRequest.Gender, userRequest.Banned, userRequest.UserStatus);

        try {
            var user = await userService.UpdateUserAsync(userId, userRequest);

            return Ok(new UpdateUserResponseDTO(
                user.Email,
                user.UserStatus,
                Request.Headers["Authorization"][0].Split(' ')[1]
            ));

        } catch (RegistrationException e) {
            if (e.Message.Equals("User does not exist")) {
                return NotFound();
            }
            return BadRequest(new { message = e.Message });
        }
    }

    [Authorize(Roles = UserRoles.UserWrite + "," + UserRoles.UserRead)]
    [HttpDelete(ApiRoutes.User.DeleteByUserId)]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId) {
        logger.LogDebug("Deleting user with data [userId = {userId}]", userId);

        try {
            var response = await userService.DeleteUserAsync(userId);
            return NoContent();
        } catch (DeleteException e) {
            if (e.Message.Equals("User does not exist")) {
                return NotFound();
            }
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost(ApiRoutes.User.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRequest) {
        logger.LogDebug("Registering user with data [email = {email}, password = {password}], age= {age}, gender= {gender}",
            userRequest.Login, userRequest.Password, userRequest.Age, userRequest.Gender);

        try {
            var user = await userService.RegisterUserAsync(userRequest);

            return Ok(new { });
            // the lines below is the proper way to return (using Created()) but spec wants us to return status code 200, perhaps
            // we can talk with other groups and change the return status to 201. keeping the code in case it's decided to use 201.
            //var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            //var locationUri = baseUrl + "/" + ApiRoutes.User.GetByEmail.Replace("{email}", user.Email);
            //return Created(locationUri, response);
        } catch (RegistrationException e) {
            return Conflict(new { message = e.Message });
        }
    }
}
