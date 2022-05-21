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
public class UsersController : UserAwareController {
    public const string TokenExpireHeader = "X-Expires-After";

    private readonly ILogger<UsersController> logger;
    private readonly IUserService userService;

    public UsersController(
        IUserService userService,
        UserManager<User> userManager,
        ILogger<UsersController> logger) : base(userManager) {
        this.userService = userService;
        this.logger = logger;
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Login)]
    public async Task<IActionResult> Login([FromBody] UserAuthenticationRequest userRequest) {
        logger.LogDebug("Authenticating user with data [email = {Login}, password = {Password}]", userRequest.Login, userRequest.Password);

        try {
            var response = await userService.AuthenticateUserAsync(userRequest);

            // append token expiration date to header
            Response.Headers.Add(TokenExpireHeader, response.TokenExpiration.ToString("O"));

            return Ok(new LoginResponseDTO(response.Token, response.User.UserStatus, response.User.Id));
        } catch (AuthenticationCredentialsException) {
            return Unauthorized(new { message = "invalid login or password" });
        } catch (AuthenticationBannedException) {
            return Unauthorized(new { message = "account has been suspended" });
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
        var user = await UserManager.FindByIdAsync(userId);

        if (user == null) {
            return NotFound();
        }

        return Ok(new UserDTO(user));
    }

    [Authorize(Roles = UserRoles.UserWrite + "," + UserRoles.UserRead)]
    [HttpPut(ApiRoutes.User.UpdateByUserId)]
    public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UserUpdateRequest userRequest) {
        var contextUser = await this.ContextUser();
        if (contextUser == null || !contextUser.IsAdmin && contextUser.Id != userId) {
            return Forbid();
        }

        logger.LogDebug("Updating user with data [login = {Login}, password = {Password}], age= {Age}, gender= {Gender}",
            userRequest.Login, userRequest.Password, userRequest.Age, userRequest.Gender);

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
        var contextUser = await this.ContextUser();
        if (contextUser == null || !contextUser.IsAdmin && contextUser.Id != userId) {
            return Forbid();
        }

        logger.LogDebug("Deleting user with data [userId = {UserId}]", userId);

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
        logger.LogDebug("Registering user with data [email = {Email}, password = {Password}], age= {Age}, gender= {Gender}",
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
