using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementApp.Models;

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Index page to list users
    [Authorize(Policy = "NotBlocked")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            user.LastLoginTime = DateTime.Now;
            await _userManager.UpdateAsync(user);
        }

        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    // Block selected users
    [HttpPost]
    public async Task<IActionResult> BlockUsers([FromBody] IEnumerable<string> userIds)
    {
        var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        foreach (var user in users)
        {
            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction("Index");
    }

    // Unblock selected users
    [HttpPost]
    public async Task<IActionResult> UnblockUsers([FromBody] IEnumerable<string> userIds)
    {
        var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        foreach (var user in users)
        {
            user.IsBlocked = false;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction("Index");
    }

    // Delete selected users
    [HttpPost]
    public async Task<IActionResult> DeleteUsers([FromBody] IEnumerable<string> userIds)
    {
        var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        foreach (var user in users)
        {
            await _userManager.DeleteAsync(user);
        }

        if (userIds.Contains(_userManager.GetUserId(User)))
        {
            await _signInManager.SignOutAsync(); // Sign out the current user
        }

        return RedirectToAction("Index");
    }
}
