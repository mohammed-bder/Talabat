﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(1 , double.MaxValue ,ErrorMessage = "Price must be greater than Zero!!")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least one!!")]
        public int Quantity { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
    }
}