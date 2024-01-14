using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth0.AspNetCore.Authentication;
using Newtonsoft.Json;
using ThreeAmigosWebsite.Services;
using ThreeAmigosWebsite.Models;
using Auth0.ManagementApi;
using Newtonsoft.Json.Linq;
using Auth0.ManagementApi.Models.Actions;
using Auth0.ManagementApi.Models.Grants;
using System.Text;

namespace ThreeAmigosWebsite.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    record managementTokenDto(string access_token, string token_type, int expires_in);

    public async Task<string> GetManagementApiTokenAsync() 
    {
        
        var tokenClient = new HttpClient();
        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var response = await tokenClient.PostAsync("oauth/token", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                {"client_id", _configuration["Auth:ClientId"]},
                {"client_secret", _configuration["Auth:ClientSecret"]},
                {"audience", _configuration["Auth:Management:Audience"]},
                {"grant_type", "client_credentials"}
            }
        ));

        var content = await response.Content.ReadAsStringAsync();
        var jsonResult = JObject.Parse(content);

        // setting up management api
        var token = jsonResult["access_token"].Value<string>();
        // management audience or my audience?
        var audience = _configuration["Auth:Management:Audience"];
        ManagementApiClient managementApiClient = new ManagementApiClient(token, audience);
        
        return token;
    }

    public async Task<string> GetUserDataAsync(string userEmailAddress)
    {
        // getting access token to auth0 management api
        var managementApiToken = await GetManagementApiTokenAsync();

        // setting up connection to the api 
        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:Management:Audience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", managementApiToken); 

        // getting my users from the api
        var apiResponse = await client.GetAsync($"users-by-email?email={userEmailAddress}");
        apiResponse.EnsureSuccessStatusCode();
        var result = await apiResponse.Content.ReadAsStringAsync();
        return result;
    }
    public async Task UpdateUserDetails(string userId, string newName, string billingAddress, string phoneNumber)
    {
        // getting access token to auth0 management api
        var managementApiToken = await GetManagementApiTokenAsync();

        // setting up connection to the api
        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:Management:Audience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", managementApiToken); 

        // updating the user details
        var newUserDetails = new 
        {
            nickname = newName,
            user_metadata = new {
                billing_address = billingAddress,
                contact_number = phoneNumber
            }
        };

        string jsonContent = JsonConvert.SerializeObject(newUserDetails);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var apiResponse = await client.PatchAsync($"users/{userId}", content);
        apiResponse.EnsureSuccessStatusCode();
        return;
    }

    public async Task DeleteUserProfile(string userId)
    {
        // getting access token to auth0 management api
        var managementApiToken = await GetManagementApiTokenAsync();

        // setting up connection to the api 
        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:Management:Audience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", managementApiToken); 

        // Deleting the profile
        var apiResponse = await client.DeleteAsync($"users/{userId}");
        apiResponse.EnsureSuccessStatusCode();
        return;
    }
}