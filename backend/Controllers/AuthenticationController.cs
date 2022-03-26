using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CbtBackend.Models;
using CbtBackend.Entities;

namespace CbtBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase {
    private readonly ILogger<AuthenticationController> logger;

    public AuthenticationController (ILogger<AuthenticationController> logger) {
        this.logger = logger;
    }

    [AllowAnonymous]
    [HttpPost(Name = "PostAuthentication")]
    public string Post(UserAuthenticationRequest authenticationRequest) {
        logger.LogInformation("authenticating user with data [username = {username}, password = {password}]", 
            authenticationRequest.Email, authenticationRequest.Password);
        return "token";
    }
}