using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThreeAmigosWebPage.Pages;
using ThreeAmigosWebPage.Models;
using Microsoft.AspNetCore.Http;

namespace ThreeAmigosWebPage.Controller;

public class AccountController : ControllerBase
{
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
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithRedirectUri("/Index")
        .Build();

        await HttpContext.SignOutAsync(
            Auth0Constants.AuthenticationScheme, authenticationProperties
        );
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }

}
