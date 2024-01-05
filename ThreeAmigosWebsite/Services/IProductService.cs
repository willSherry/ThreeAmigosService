using System;
using System.Threading.Tasks;

namespace ThreeAmigosWebsite.Services;
public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductDataAsync();
}