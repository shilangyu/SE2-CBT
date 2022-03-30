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

    [HttpPost(ApiRoutes.User.Register)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRequest) {
        // check if user already exists
        var existingUser = await userService.GetUserByEmailAsync(userRequest.Email);
        if (existingUser != null) {
            return BadRequest(new { message = "User already exists" });
        }

        logger.LogDebug("Registering user with data [email = {email}, password = {password}]", userRequest.Email, userRequest.Password);

        var user = new User {
            Email = userRequest.Email,
            Password = userRequest.Password,
            Banned = userRequest.Banned,
            Age = userRequest.Age,
            Gender = userRequest.Gender,
            UserStatus = 1, // what is this used for???
            Roles = new List<string> {UserRoles.UserWrite}
        };

        // add user to db
        var registered = await userService.RegisterUserAsync(user);
        if (!registered) {
            return BadRequest(new { message = "Operation failed" });
        }

        return Ok();
        // the lines below is the proper way to return (using Created()) but spec wants us to return status code 200, perhaps
        // we can talk with other groups and change the return status to 201
        //var response = new UserRegistrationResponse() { User = user };
        //var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        //var locationUri = baseUrl + "/" + ApiRoutes.User.GetByEmail.Replace("{email}", user.Email);
        //return Created(locationUri, response);
    }
}
