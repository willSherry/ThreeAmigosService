using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using ThreeAmigosWebPage.Services;
using ThreeAmigos.Products.Services.UnderCutters;

namespace ThreeAmigosWebPage.Pages
{
    public class IndexModel : PageModel
{
    private readonly IUnderCuttersService _underCuttersService;

    public IndexModel(IUnderCuttersService underCuttersService)
    {
        _underCuttersService = underCuttersService;
    }

    public List<ThreeAmigos.Products.Services.UnderCutters.ProductDto> Products { get; private set; }

    // public async Task OnGetAsync()
    // {
    //     try
    //     {
    //         // Fetch product data using ProductService
    //         Products = await _productService.GetProductDataAsync();
    //     }
    //     catch (Exception ex)
    //     {
    //         // Handle exception
    //     }
    // }

    public async Task OnGetAsync(string search)
    {
        try 
        {
            // Checks that search request isnt empty
            if (!string.IsNullOrWhiteSpace(search))
            {
                var products = await _underCuttersService.GetProductsAsync();
                Products = products.Where(p => 
                p.Name.ToLower().Contains(search) ||
                p.Description.ToLower().Contains(search) ||
                p.CategoryName.ToLower().Contains(search) ||
                p.BrandName.ToLower().Contains(search)
                ).ToList();    
                
            }
            else
            {
                // Display all products if search is made while search box is empty
                var allProducts = await _underCuttersService.GetProductsAsync();
                Products = allProducts.ToList();
            }
        }
        catch (Exception e)
        {
                throw;
        }
    }
}
}