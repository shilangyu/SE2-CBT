using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CbtBackend.Models;
using CbtBackend.Entities;
using CbtBackend.Services;

namespace CbtBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase {
    private readonly ILogger<AuthenticationController> logger;
    private readonly IUserService userService;

    public AuthenticationController (IUserService userService, ILogger<AuthenticationController> logger) {
        this.logger = logger;
        this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost(Name = "PostAuthentication")]
    public IActionResult Post(UserAuthenticationRequest authenticationRequest) {
        logger.LogInformation("authenticating user with data [email = {email}, password = {password}]", 
            authenticationRequest.Email, authenticationRequest.Password);
        
        try {
            var token = userService.Authenticate(authenticationRequest);
            return Ok(token);
        } catch (AuthenticationCredentialsException) {
            return Unauthorized(new { message = "invalid username or password" }); 
        }
    }
}