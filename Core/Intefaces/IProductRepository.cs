using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Intefaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsyn(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();

    }
}