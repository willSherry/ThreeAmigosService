using Microsoft.AspNetCore.Mvc;
using ThreeAmigos.Products.Services.ProductsRepo;
using ThreeAmigos.Products.Services.UnderCutters;

namespace ThreeAmigos.Products.Controllers;

[ApiController]
[Route("[controller]")]

public class DebugController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IUnderCuttersService _underCuttersService;

    private readonly IProductsRepo _productsRepo;

    public DebugController(ILogger<DebugController> logger,
                            IUnderCuttersService underCuttersService,
                            IProductsRepo productsRepo)
    {
        _logger = logger;
        _underCuttersService = underCuttersService;
        _productsRepo = productsRepo;
    }                        

    // GET: /debug/undercutters
    [HttpGet("UnderCutters")]
    public async Task<IActionResult> UnderCutters()
    {
        IEnumerable<ProductDto> products = null;
        try
        {
            products = await _underCuttersService.GetProductsAsync();
        }
        catch
        {
            _logger.LogWarning("Exception occurred using UnderCutters Service.");
            products = Array.Empty<ProductDto>();
        }
        return Ok(products.ToList());
    }

     // GET: /debug/repo
    [HttpGet("repo")]
    public async Task<IActionResult> Repo()
    {
        IEnumerable<Product> products = null;
        try
        {
            products = await _productsRepo.GetProductsAsync();
        }
        catch
        {
            _logger.LogWarning("Exception occurred using Products repo.");
            products = Array.Empty<Product>();
        }
        return Ok(products.ToList());
    }   
}