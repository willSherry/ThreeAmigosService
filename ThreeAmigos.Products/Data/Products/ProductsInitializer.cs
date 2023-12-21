using System;

namespace ThreeAmigos.Products.Data.Products;

public static class ProductsInitializer
{
    public static async Task SeedTestData(ProductsContext context)
    {
        if(context.Products.Any())
        {
            return;
        }

        // If database is empty, add test data
        var products = new List<Product>
        {
            new() {Id = 1, Name = "Test product 6"},
            new() {Id = 2, Name = "Test product 7"},
            new() {Id = 3, Name = "Test product 8"},
        };
        products.ForEach(p => context.Add(p));
        await context.SaveChangesAsync();
    }
}