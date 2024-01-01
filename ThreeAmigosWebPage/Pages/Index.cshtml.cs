using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using ThreeAmigosWebPage.Services;

namespace ThreeAmigosWebPage.Pages
{
    public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public List<ProductDto> Products { get; private set; }

    public async Task OnGetAsync(string search)
    {
        try 
        {
            // Checks that search request isnt empty
            if (!string.IsNullOrWhiteSpace(search))
            {
                var products = await _productService.GetProductDataAsync();
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
                Products = await _productService.GetProductDataAsync();
            }
        }
        catch (Exception e)
        {
                throw;
        }
    }
}
}