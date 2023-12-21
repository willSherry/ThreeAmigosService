using System;
using Microsoft.EntityFrameworkCore;
using ThreeAmigos.Products.Data.Products;

namespace ThreeAmigos.Products.Services.ProductsRepo;

public class ProductsRepo : IProductsRepo
{
private readonly ProductsContext _productsContext;

public ProductsRepo(ProductsContext productsContext)
{
    _productsContext = productsContext;
}

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var products = await _productsContext.Products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name
        }).ToListAsync();
        return products;
    }
}