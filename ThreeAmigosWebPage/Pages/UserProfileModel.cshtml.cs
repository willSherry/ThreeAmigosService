using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThreeAmigosWebPage.Models;

namespace ThreeAmigosWebPage.Pages;

public class UserProfileModel : PageModel
{
    private readonly ILogger<UserProfileModel> _logger;

    public UserProfileModel(ILogger<UserProfileModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        UserProfileViewModel model = new UserProfileViewModel()
        {
            Name = User.Identity.Name,
            EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
        };
    }
}

