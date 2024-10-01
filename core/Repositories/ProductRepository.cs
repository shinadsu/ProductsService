using core.Data;
using core.Models;
using core.Models.CreateProductExtension;
using core.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace core.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private DataContext _dataContext { get; set; }

        public ProductRepository(DataContext dataContext) { _dataContext = dataContext; }
       

        public async Task DeleteProduct(string name)
        {
            try
            {
                var user = await _dataContext.Products.FirstOrDefaultAsync(p => p.Name == name);
                _dataContext.Products.Remove(user);
                await _dataContext.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                throw new Exception($"Fatal error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fatal error:{ex.Message}");
            }
        }


        public async Task<Product> GetProductByName(string productName)
        {
            try
            {
                if (productName == null) throw new ArgumentNullException(nameof(productName), "name cannot be null");
                return await _dataContext.Products
                    .Include(p => p.Versions)
                    .FirstOrDefaultAsync(p => p.Name == productName);
            }
            catch (DbException dbEx)
            {
                throw new Exception($"Fatal error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fatal error:{ex.Message}");
            }
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                return await _dataContext.Products
                .Include(p => p.Versions)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (DbException dbEx)
            {
                throw new Exception($"Fatal error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fatal error:{ex.Message}");
            }
        }


        public async Task PatchProduct(string name, Product newProduct)
        {
            try
            {
                var existingProduct = await _dataContext.Products
                    .Include(p => p.Versions) // Загружаем связанные версии
                    .FirstOrDefaultAsync(p => p.Name == name);

                if (existingProduct == null)
                    throw new ArgumentNullException(nameof(existingProduct), "Product name is invalid");

                // Обновляем основные данные продукта
                existingProduct.Name = newProduct.Name;
                existingProduct.Description = newProduct.Description;

                // Обновляем версии
                foreach (var version in newProduct.Versions) // 1.1 
                {
                    var existingVersion = existingProduct.Versions
                        .FirstOrDefault(v => v.Id == version.Id); // 1.1 & 1.0

                    if (existingVersion != null)
                    {
                        // Обновляем существующую версию
                        existingVersion.Name = version.Name;
                        existingVersion.Description = version.Description;
                        existingVersion.CreatingDate = version.CreatingDate;
                        existingVersion.Width = version.Width;
                        existingVersion.Height = version.Height;
                        existingVersion.Length = version.Length;
                    }
                    else
                    {
                        // Добавляем новую версию
                        existingProduct.Versions.Add(new ProductVersion
                        {
                            Id = version.Id,
                            ProductID = existingProduct.Id,
                            Name = version.Name,
                            Description = version.Description,
                            CreatingDate = version.CreatingDate,
                            Width = version.Width,
                            Height = version.Height,
                            Length = version.Length
                        });
                    }
                }

                // Удаляем версии, которые больше не представлены в обновлении
                var versionIdsToKeep = newProduct.Versions.Select(v => v.Id).ToList();

                // Используем цикл для удаления
                foreach (var existingVersion in existingProduct.Versions.ToList())
                {
                    if (!versionIdsToKeep.Contains(existingVersion.Id))
                    {
                        _dataContext.ProductVersions.Remove(existingVersion); 
                    }
                }

                await _dataContext.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                throw new Exception($"Fatal error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fatal error: {ex.Message}");
            }
        }




        public async Task PostProduct(Product postProduct)
        {
            if (postProduct == null) throw new ArgumentNullException(nameof(postProduct));

            try
            {
                await _dataContext.Products.AddAsync(postProduct);
                await _dataContext.SaveChangesAsync();
            }
            catch (DbException Dbex)
            {
                throw new Exception($"Fatal error: {Dbex.InnerException?.Message ?? Dbex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fatal error:{ex.Message}");
            }
        }
    }
}
