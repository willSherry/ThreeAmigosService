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



    public async Task<dynamic> UserInfo()
    {
        var userEmail = User.Identity.Name;
        if (userEmail == null)
        {
            return null;
        }

        var userData = await _userService.GetUserDataAsync(userEmail);
        // Deserialize the JSON string into a dynamic object
        dynamic userObject = JsonConvert.DeserializeObject<List<dynamic>>(userData)[0];
        return userObject;
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
    public async Task<IActionResult> Profile()
    {
        dynamic userInfo = await UserInfo();

        string userName = userInfo.nickname;
        string profilePicture = userInfo.picture;

        return View(new UserProfileViewModel()
        {
            Name = userName,
            EmailAddress = User.Identity.Name,
            ProfileImage = profilePicture
        });
    }

    [Authorize]
    public async Task<IActionResult> UpdateProfile(string Name)
    {
        var userEmail = User.Identity.Name;
        if (userEmail == null)
        {
            return BadRequest("Email is null");
        }
        
        var userData = await _userService.GetUserDataAsync(userEmail);

        // Deserialize the JSON string into a dynamic object
        dynamic userObject = JsonConvert.DeserializeObject<List<dynamic>>(userData)[0];

        // Access the 'user_id' property
        string userId = userObject.user_id;
        string nickname = Name;
        
        // IT WORKS, now just to get the string value from the edit page
        /*
            what to do, pass all parameters through no matter what, if one is null,
            set it to be the original value, bosh   
        */ 
        
        

        await _userService.UpdateUserDetails(userId, nickname);

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