using System;
using System.Net;

namespace ThreeAmigos.Products.Services.UnderCutters;

public class UnderCuttersService : IUndercutterService
{
    private readonly HttpClient _client;

    public UnderCuttersService(HttpClient client, 
                            IConfiguration configuration)
    {
        var baseURL = configuration["WebServices:UnderCutters:BaseURL"] ?? "";
        client.BaseAddress = new System.Uri(baseURL);
        client.Timeout = TimeSpan.FromSeconds(5);
        client.DefaultRequestHeaders.Add("Accept", "application.json");
        _client = client;
    }
    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var uri = "api/product";
        var response = await _client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var reviews = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
        return reviews;
    }
}