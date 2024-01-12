using System;
using System.Threading.Tasks;
using ThreeAmigosWebsite.Models;

namespace ThreeAmigosWebsite.Services;
public interface IUserService
{
    Task<IEnumerable<UserProfileViewModel>> GetUserDataAsync(string userEmailAddress);
}