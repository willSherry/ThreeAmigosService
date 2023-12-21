using System;

namespace ThreeAmigos.Products.Services.UnderCutters;

public interface IUnderCuttersService 
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}