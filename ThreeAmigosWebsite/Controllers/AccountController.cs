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

        Random r = new Random();
        int maxPossibleFunds = 450;
        double userFunds = r.NextDouble() * maxPossibleFunds;
        double roundedFunds = (double)Math.Round(userFunds, 2);

        string billingAddress = userInfo?.user_metadata?.billing_address;
        string phoneNumber = userInfo?.user_metadata?.contact_number;
        return View(new UserProfileViewModel()
        {
            Name = userName,
            EmailAddress = User.Identity.Name,
            ProfileImage = profilePicture,
            Funds = roundedFunds,
            BillingAddress = billingAddress,
            PhoneNumber = phoneNumber
        });
    }

    [Authorize]
    public async Task<IActionResult> UpdateProfile(string? Name, string? BillingAddress, string? PhoneNumber)
    {
        dynamic userObject = await UserInfo();

        BillingAddress ??= userObject?.user_metadata?.billing_address;
        PhoneNumber ??= userObject?.user_metadata?.contact_number;
        Name ??= userObject?.nickname;
        
        if (Name == null)
        {
            Name = userObject.nickname;
        } 

        // Access the 'user_id' property
        string userId = userObject.user_id;
        string nickname = Name;
        string billingAddress = BillingAddress;
        string phoneNumber = PhoneNumber;
        
        await _userService.UpdateUserDetails(userId, nickname, billingAddress, phoneNumber);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> DeleteProfile()
    {
        dynamic userObject = await UserInfo();

        // Access the 'user_id' property
        string userId = userObject.user_id;

        // Deleting the user profile
        _userService.DeleteUserProfile(userId);

        await Logout();

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