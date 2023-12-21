using System;

namespace ThreeAmigos.Products.Services.UnderCutters;

public interface IUndercutterService{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}