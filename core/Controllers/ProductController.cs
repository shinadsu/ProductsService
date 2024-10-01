using Azure.Core;
using core.Models;
using core.Models.CreateProductExtension;
using core.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data.Common;

namespace core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private IProductRepository _repository;


        public ProductController(IProductRepository productRepository) { _repository = productRepository; }


        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
               return await _repository.GetProducts();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("name")]
        []
        public async Task<IActionResult> DeleteProductAsync(string name)
        {
            try
            {
                await _repository.DeleteProduct(name);
                return Ok();
            }
            catch (DbException dbEx)
            {
                return BadRequest($"Database invalid: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("name")]
        public async Task<Product> GetProductAsync(string name)
        {
            try
            {
                return await _repository.GetProductByName(name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]
        // В запросе используется другая модел для того, чтобы POST запрос просто не показывал id. Так как GUID должен генерироваться самостоятельно
        public async Task<IActionResult> PostProductAsync(CreateProductRequest request)
        {   
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Логика состоит в том, что Id должны быть уникальными, но не ProductID который должен ссылаться на Id продукта
                var guid_for_id_and_productId = new Guid();

                var product = new Product
                {
                    Id = guid_for_id_and_productId, // Генерируем уникальный ID
                    Name = request.Name,
                    Description = request.Description,
                    Versions = request.Versions.Select(v => new ProductVersion
                    {   
                        Id = Guid.NewGuid(), // Генерируем уникальный ID для версии
                        ProductID = guid_for_id_and_productId, // тот же самый id что и у ID продукта
                        Name = v.Name,
                        Description = v.Description,
                        CreatingDate = v.CreatingDate,
                        Width = v.Width,
                        Height = v.Height,
                        Length = v.Length
                    }).ToList()
                };


                await _repository.PostProduct(product);
                return Ok(product);
            }
            catch (DbException dbEx)
            {
                return BadRequest($"Database invalid: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }


        [HttpPatch("name")]
        // Так как имя уникальное, то в целом можно использовать его как ключ обвноления продукта
        public async Task<IActionResult> PatchProductAsync(string name, Product product)
        {
            try
            {
                await _repository.PatchProduct(name, product);
                return Ok("product was succesfully updated");
            }
            catch (DbException dbEx)
            {
                return BadRequest($"Database invalid: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex )
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }


    }
}
