using System.Net.Http.Headers;
using Auth0.AspNetCore.Authentication;
using Newtonsoft.Json;
using ThreeAmigosWebPage.Services;


namespace ThreeAmigosWebPage.Services;

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

    public async Task<List<ProductDto>> GetProductDataAsync()
    {
        var tokenClient = _clientFactory.CreateClient();

        //Auth0 Authority
        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var tokenParams = new Dictionary<string, string> {
            { "grant_type", "client_credentials" },
            { "client_id", _configuration["Auth:ClientId"] },
            { "client_secret", _configuration["Auth:ClientSecret"] },
            { "audience", _configuration["Auth:Audience"] },
        };
        var tokenForm = new FormUrlEncodedContent(tokenParams);
        var tokenResponse = await tokenClient.PostAsync("/oauth/token", tokenForm);
        tokenResponse.EnsureSuccessStatusCode();
        var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

        // FIX: token should be cached and not called every time

        var client = _clientFactory.CreateClient();

        var serviceBaseAddress = _configuration["Services:BaseAddress"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);
        var response = await client.GetAsync("debug/undercutters");
        response.EnsureSuccessStatusCode();
        List<ProductDto> result = await response.Content.ReadAsAsync<List<ProductDto>>();
        return result;
    }

}