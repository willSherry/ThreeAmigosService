using Newtonsoft.Json;
using ThreeAmigosWebPage.Services;


namespace ThreeAmigosWebPage.Services;

    public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<ProductDto>> GetProductDataAsync()
    {
        try
        {
            var url = "https://threeamigosservice.azurewebsites.net/debug/undercutters"; // Replace with your actual URL

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(json);
                return products;
            }
            else
            {
                // Handle the case when the response is not successful
                // For simplicity, returning an empty list here
                return new List<ProductDto>();
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., network issues, timeouts, etc.)
            throw new Exception("Error fetching product data", ex);
        }
    }
}