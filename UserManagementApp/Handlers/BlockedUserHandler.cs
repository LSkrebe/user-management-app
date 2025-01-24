using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UserManagementApp.Models;

public class BlockedUserHandler : AuthorizationHandler<BlockedUserRequirement>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public BlockedUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BlockedUserRequirement requirement)
    {
        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        var user = await _userManager.GetUserAsync(context.User);
        if (user != null && user.IsBlocked)
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement); // Use 'requirement' here
        }
    }
}