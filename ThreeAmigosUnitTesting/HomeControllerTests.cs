using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;
using ThreeAmigos.Products.Controllers;
using ThreeAmigosWebsite.Controllers;
using ThreeAmigosWebsite.Models;
using ThreeAmigosWebsite.Services;

namespace ThreeAmigosUnitTesting;

[TestClass]
public class HomeControllerTests
{

    private HomeController _homeController;
    private Mock<ILogger<HomeController>> _loggerMock;
    private Mock<IProductService> _productService;

    [TestInitialize]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        _productService = new Mock<IProductService>();

        _homeController = new HomeController(_loggerMock.Object, _productService.Object);
    }

    [TestMethod]
    public async Task IndexTest()
    {
        var result = await _homeController.Index() as ViewResult;

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Model, typeof(ProductDTO[]));
        var model = (ProductDTO[])result.Model;
        Assert.AreEqual(0, model.Length); 
    }

}
