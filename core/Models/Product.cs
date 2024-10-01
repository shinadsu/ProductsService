using System.ComponentModel.DataAnnotations;

namespace core.Models
{
    public class Product
    {   

        [Key]
        [Required(ErrorMessage = "Please enter product identity.")]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Please enter product name.")]
        [StringLength(255)]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        
        [Display(Name = "Product Description")]
        public string? Description { get; set; }


        public ICollection<ProductVersion> Versions { get; set; }
    }
}
