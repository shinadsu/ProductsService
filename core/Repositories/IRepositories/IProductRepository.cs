using core.Models;
using core.Models.CreateProductExtension;

namespace core.Repositories.IRepositories
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetProducts();


        Task<Product> GetProductByName(string productName);


        Task PostProduct(Product postProduct);


        Task PatchProduct(string name, Product patchProduct);


        Task DeleteProduct(string name);
        
    }
}
