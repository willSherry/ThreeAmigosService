using System.Net.WebSockets;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ThreeAmigos.Products.Controllers;
using ThreeAmigos.Products.Services.ProductsRepo;
using ThreeAmigos.Products.Services.UnderCutters;

namespace ThreeAmigosUnitTesting;

[TestClass]
public class DebugControllerTests
{
        private DebugController _debugController;
        private Mock<ILogger<DebugController>> _loggerMock;
        private Mock<IUnderCuttersService> _underCuttersServiceMock;
        private Mock<IProductsRepo> _productsRepoMock;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<DebugController>>();
            _underCuttersServiceMock = new Mock<IUnderCuttersService>();
            _productsRepoMock = new Mock<IProductsRepo>();

            _debugController = new DebugController(_loggerMock.Object, _underCuttersServiceMock.Object, _productsRepoMock.Object);
        }

        [TestMethod]
        public async Task UnderCuttersTest()
        {
            var expectedProducts = new List<ProductDto>();
            _underCuttersServiceMock.Setup(c => c.GetProductsAsync())
            .Returns(Task.FromResult<IEnumerable<ProductDto>>(expectedProducts));

            var result = await _debugController.UnderCutters();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            var actualProducts = okResult.Value as List<ProductDto>;
            CollectionAssert.AreEqual(expectedProducts, actualProducts);
        }

        [TestMethod]
        public async Task RepoTest()
        {
            var expectedProducts = new List<Product>();
            _productsRepoMock.Setup(c => c.GetProductsAsync())
            .Returns(Task.FromResult<IEnumerable<Product>>(expectedProducts));

            var result = await _debugController.Repo();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            var actualProducts = okResult.Value as List<Product>;
            CollectionAssert.AreEqual(expectedProducts, actualProducts);
        }
}