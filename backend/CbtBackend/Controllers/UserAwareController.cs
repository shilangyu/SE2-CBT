using Microsoft.AspNetCore.Mvc;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Identity;

namespace CbtBackend.Controllers;

public class UserAwareController : ControllerBase {
    public readonly UserManager<User> UserManager;

    public UserAwareController(UserManager<User> userManager) {
        UserManager = userManager;
    }
}

public static class UserAwareControllerExtensions {
    public static async Task<User?> ContextUser(this UserAwareController self) {
        return await self.UserManager.GetUserAsync(self.User);
    }
}
