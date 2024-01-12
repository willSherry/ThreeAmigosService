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

namespace ThreeAmigosWebsite.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    record managementTokenDto(string access_token, string token_type, int expires_in);

    public async Task<string> GetUserDataAsync(string userEmailAddress)
    {
        // getting access token to auth0 management api

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

        // getting my users from the api
        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:Management:Audience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token); //token?.access_token maybe?
        var apiResponse = await client.GetAsync($"users-by-email?email={userEmailAddress}");
        apiResponse.EnsureSuccessStatusCode();
        var result = await apiResponse.Content.ReadAsStringAsync();
        return result;
    }
}