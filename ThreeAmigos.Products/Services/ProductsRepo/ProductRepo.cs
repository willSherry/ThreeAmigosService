using System;
using Microsoft.EntityFrameworkCore;
using ThreeAmigos.Products.Data.Products;

namespace ThreeAmigos.Products.Services.ProductsRepo;

public class ProductsRepo : IProductsRepo
{
    // Db context stored as local variable
    private readonly ProductsContext _productsContext;

    // Dependancy injection used to receive product context
    public ProductsRepo(ProductsContext productsContext)
    {
        _productsContext = productsContext;
    }

    // Implement in interface to get all the products
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