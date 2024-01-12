using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeAmigosWebsite.Models;
using ThreeAmigosWebsite.Controllers;
using ThreeAmigosWebsite.Services;
using Newtonsoft.Json;

namespace ThreeAmigosWebsite.Controllers;

public class AccountController : Controller
{
    private readonly IManagementApiClient _managementApiClient;
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    
    public async Task Login(string returnUrl = "/")
    {
        var authenticationProperties = new
            LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

        await HttpContext.ChallengeAsync(
            Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [Authorize]
    public async Task Logout()
    {
        var authenticationProperties = new
            LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

        await HttpContext.SignOutAsync(
            Auth0Constants.AuthenticationScheme, authenticationProperties);

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    public IActionResult Profile()
    {
        return View(new UserProfileViewModel()
        {
            Name = User.Identity.Name,
            EmailAddress = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            ProfileImage = User.Claims
                .FirstOrDefault(c => c.Type == "picture")?.Value
        });
    }

    // [Authorize]
    // public async Task<IActionResult> UpdateProfile(string userEmail)
    // {
    //     userEmail = User.Identity.Name;
    //     // MAY NOT NEED PARAMETER, GET EMAIL FROM IN HERE? LOOK AT PROFILE ACTION
    //     if (userEmail == null)
    //     {
    //         return BadRequest("STILLLLLL NULL");
    //     }
    //     var userId = await _userService.GetUserDataAsync(userEmail);
    //     Console.WriteLine(userId);
    //     return RedirectToAction("Index", "Home");
    // }
    [Authorize]
    public async Task<IActionResult> UpdateProfile(string userEmail)
    {
        userEmail = User.Identity.Name;
        if (userEmail == null)
        {
            return BadRequest("Email is null");
        }

        var userData = await _userService.GetUserDataAsync(userEmail);

        // Deserialize the JSON string into a dynamic object
        dynamic userObject = JsonConvert.DeserializeObject<List<dynamic>>(userData)[0];

        // Access the 'user_id' property
        string userId = userObject.user_id;

        Console.WriteLine(userId);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult EditProfile()
    {
        return View();
    }

    [Authorize]
    public IActionResult Claims()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}