using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmallRestaurantApp.Models
{
  public class Comment
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }

    public int DishId { get; set; }
    public Dish? Dish { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Text { get; set; }

    public DateTime ReviewDate { get; set; }
  }
}