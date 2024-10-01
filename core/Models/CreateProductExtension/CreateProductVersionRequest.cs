﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.Models.CreateProductExtension
{
    public class CreateProductVersionRequest
    {

        [Required(ErrorMessage = "Please enter version name.")]
        [Display(Name = "Product version name")]
        [StringLength(255)]
        public string Name { get; set; }


        [Display(Name = "Product version Description")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Please enter date.")]
        [Display(Name = "Product version date")]
        public DateTime CreatingDate { get; set; }


        [Required(ErrorMessage = "Please enter product size")]
        [Display(Name = "Product width")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Width must be a positive number.")]
        public decimal Width { get; set; }


        [Required(ErrorMessage = "Please enter product size")]
        [Display(Name = "Product height")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Height must be a positive number.")]
        public decimal Height { get; set; }


        [Required(ErrorMessage = "Please enter product size")]
        [Display(Name = "Product length")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Length must be a positive number.")]
        public decimal Length { get; set; }
    }
}
