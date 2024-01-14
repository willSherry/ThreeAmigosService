using System.ComponentModel.DataAnnotations;

namespace ThreeAmigosWebsite.Models;

public class UserProfileViewModel
{
    public string EmailAddress { get; set; }

    public string Name { get; set; }

    public string ProfileImage { get; set; }

    [Required]
    public string? BillingAddress { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }
}