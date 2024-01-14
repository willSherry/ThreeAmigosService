namespace ThreeAmigosWebsite.Models;

public class UserProfileViewModel
{
    public string EmailAddress { get; set; }

    public string Name { get; set; }

    public string ProfileImage { get; set; }

    public string? BillingAddress { get; set; }

    public string? PhoneNumber { get; set; }
}