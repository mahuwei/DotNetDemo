using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicallyDbConnectionString.Models {
  public class Employee : Entity {
    public Guid CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }

    [Required]
    [MaxLength(10)]
    public string WorkNo { get; set; } = null!;

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [DefaultValue(0)]
    public decimal Salary { get; set; }
  }
}