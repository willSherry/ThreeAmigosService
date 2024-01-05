using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThreeAmigosWebsite.Models;
using ThreeAmigosWebsite.Services;

namespace ThreeAmigosWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _products;

    public HomeController(ILogger<HomeController> logger, IProductService products)
    {
        _logger = logger;
        _products = products;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _products.GetProductDataAsync();
        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
