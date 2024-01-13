using System;
using System.Threading.Tasks;
using ThreeAmigosWebsite.Models;

namespace ThreeAmigosWebsite.Services;
public interface IUserService
{
    Task<string> GetUserDataAsync(string userEmailAddress);

    Task UpdateUserDetails(string userId, string newName);

    Task DeleteUserProfile(string userId);
}