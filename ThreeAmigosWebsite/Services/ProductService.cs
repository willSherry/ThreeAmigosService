using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth0.AspNetCore.Authentication;
using Newtonsoft.Json;
using ThreeAmigosWebsite.Services;



namespace ThreeAmigosWebsite.Services;

    public class ProductService : IProductService
{

    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public ProductService(IHttpClientFactory clientFactory, 
                            IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;

    }
    record TokenDto(string access_token, string token_type, int expires_in);

    public async Task<IEnumerable<ProductDTO>> GetProductDataAsync()
    {
        var tokenClient = _clientFactory.CreateClient();

        //Auth0 Authority
        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var tokenParams = new Dictionary<string, string> {
            { "grant_type", "client_credentials" },
            { "client_id", _configuration["Auth:ClientId"] },
            { "client_secret", _configuration["Auth:ClientSecret"] },
            { "audience", _configuration["Services:Products:AuthAudience"] },
        };
        var tokenForm = new FormUrlEncodedContent(tokenParams);
        var tokenResponse = await tokenClient.PostAsync("/oauth/token", tokenForm);
        tokenResponse.EnsureSuccessStatusCode();
        var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

        // FIX: token should be cached and not called every time

        var client = _clientFactory.CreateClient();

        var serviceBaseAddress = _configuration["Services:Products:BaseAddress"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);
        var response = await client.GetAsync("debug/undercutters");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>();
        return result;
    }
}