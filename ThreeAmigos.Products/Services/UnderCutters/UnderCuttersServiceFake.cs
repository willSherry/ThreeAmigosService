using System;

namespace ThreeAmigos.Products.Services.UnderCutters;

public class UnderCuttersServiceFake : IUnderCuttersService
{
    private readonly ProductDto[] _products = 
    {
        new ProductDto { Id = 1, Name = "Fake Product A"},
        new ProductDto { Id = 2, Name = "Fake Product B"},
        new ProductDto { Id = 3, Name = "Fake Product C"},
    };

    public Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var products = _products.AsEnumerable();
        return Task.FromResult(products);
    }
}