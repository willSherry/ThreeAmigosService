namespace ThreeAmigosWebsite.Models;

public class UserProfileViewModel
{
    public string EmailAddress { get; set; }

    public string Name { get; set; }

    public string ProfileImage { get; set; }

    public string? NewEmailAddress { get; set; }

    public string? NewName { get; set; }

    public string? NewProfileImage { get; set; }
}