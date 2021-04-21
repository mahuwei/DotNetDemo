using System;
using System.ComponentModel.DataAnnotations;

namespace DynamicallyDbConnectionString.Models {
  public abstract class Entity {
    [Key]
    public Guid Id { get; set; }

    [Timestamp]
    public byte[]? RowFlag { get; set; }
  }
}