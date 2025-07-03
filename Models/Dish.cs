using System.ComponentModel.DataAnnotations;
namespace SmallRestaurantApp.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}