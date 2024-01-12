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

    [Authorize]
    public async Task<IActionResult> UpdateProfile(string userEmail)
    {
        if (userEmail == null)
        {
            return BadRequest("STILLLLLL NULL");
        }
        try
        {
            var userId = await _userService.GetUserDataAsync(userEmail);
        }
        catch(Exception e)
        {
            return BadRequest("ERROR OOPS");
        }
        return Ok();
    }

    [Authorize]
    public IActionResult EditProfile()
    {
        return View();
    }

    // [Authorize]
    // public async Task<IActionResult> UpdateProfile(EditUserProfile model)
    // {
    //     try
    //     {
    //         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     }   
    //     catch(Exception e)
    //     {
    //         throw e;
    //     }
    // }


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