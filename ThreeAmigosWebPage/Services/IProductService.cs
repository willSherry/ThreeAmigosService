using System;
using System.Threading.Tasks;

namespace ThreeAmigosWebPage.Services;
public interface IProductService
{
    Task<List<ProductDto>> GetProductDataAsync();
}