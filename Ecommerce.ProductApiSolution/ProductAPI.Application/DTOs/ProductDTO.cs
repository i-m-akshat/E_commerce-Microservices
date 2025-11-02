
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Application.DTOs
{
  public record ProductDTO
    (
      int id,
      [Required]string name,
      [Required]string description,
      [Required,Range(1,int.MaxValue)] int Quantity,
      [Required,DataType(DataType.Currency)]decimal Price
    );
}
