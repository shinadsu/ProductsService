using System.ComponentModel.DataAnnotations;

namespace core.Models.CreateProductExtension
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Please enter product name.")]
        [StringLength(255)]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Display(Name = "Product Description")]
        public string? Description { get; set; }

        public List<CreateProductVersionRequest> Versions { get; set; }
    }
}
