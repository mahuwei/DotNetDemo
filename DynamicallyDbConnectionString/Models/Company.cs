using System.ComponentModel.DataAnnotations;

namespace DynamicallyDbConnectionString.Models {
  public class Company : Entity {
    [Required]
    [MaxLength(10)]
    public string No { get; set; } = null!;

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = null!;
  }
}