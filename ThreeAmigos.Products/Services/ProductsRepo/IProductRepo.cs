using System;

namespace ThreeAmigos.Products.Services.ProductsRepo;

public interface IProductsRepo
{
    Task<IEnumerable<Product>> GetProductsAsync();
}