using Newtonsoft.Json;
using ThreeAmigosWebPage.Services;
using ThreeAmigos.Products.Services.UnderCutters;
using Microsoft.AspNetCore.Mvc;

namespace ThreeAmigosWebPage.Services;

    public class ProductService 
{

    private readonly IUnderCuttersService _underCuttersService;

    public ProductService(HttpClient httpClient, IUnderCuttersService underCuttersService)
    {
        _underCuttersService = underCuttersService;
    }

    public async Task<List<ThreeAmigos.Products.Services.UnderCutters.ProductDto>> GetUnderCuttersProducts()
    {
        IEnumerable<ThreeAmigos.Products.Services.UnderCutters.ProductDto> products = null;
        try
        {
            products = await _underCuttersService.GetProductsAsync();
        }
        catch
        {
            products = Array.Empty<ThreeAmigos.Products.Services.UnderCutters.ProductDto>();
        }
        return products.ToList();
    }

    // public async Task<List<ProductDto>> GetProductDataAsync()
    // {
    //     try
    //     {
    //         var url = "https://threeamigosservice.azurewebsites.net/debug/undercutters"; // Replace with your actual URL

    //         HttpResponseMessage response = await _httpClient.GetAsync(url);

    //         if (response.IsSuccessStatusCode)
    //         {
    //             string json = await response.Content.ReadAsStringAsync();
    //             List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(json);
    //             return products;
    //         }
    //         else
    //         {
    //             // Handle the case when the response is not successful
    //             // For simplicity, returning an empty list here
    //             return new List<ProductDto>();
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         // Handle exceptions (e.g., network issues, timeouts, etc.)
    //         throw new Exception("Error fetching product data", ex);
    //     }
    // }
}
