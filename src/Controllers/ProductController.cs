using Azure.Core;
using core.Models;
using core.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class ProductController : Controller
    {   
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository) 
        {
            _logger = logger;
            _productRepository = productRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string productName)
        {
            try
            {
                if (!string.IsNullOrEmpty(productName))
                {
                    // Получаем только один продукт по имени
                    var product = await _productRepository.GetProductByName(productName);

                    if (product != null)
                    {
                        return View(new List<Product> { product }); // возвращаем в виде списка
                    }
                    else
                    {
                        return View(new List<Product>()); // Возвращаем пустой список, если продукт не найден
                    }
                }

                // Если productName не указан, возвращаем все продукты
                var products = await _productRepository.GetProducts();
                return View(products);
            }
            catch (Exception ex)
            {
               
                return View("Error"); // Возвращаем представление об ошибке
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product request)
        {
            if (ModelState.IsValid)
            {

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


                await _productRepository.PostProduct(product); 

                return Ok(); 
            }

            return BadRequest(ModelState); 
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.PatchProduct(model.Name, model);
                    return Ok(); 
                }
                catch (Exception ex)
                {
                    
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return BadRequest(ModelState); 
        }



    }
}
