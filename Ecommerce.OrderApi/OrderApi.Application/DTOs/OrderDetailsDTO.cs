
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
public record OrderDetailsDTO
    (
    [Required] int orderId,
    [Required] int productId,
    [Required] int clientId,
    [Required,EmailAddress] string clientEmail,
    [Required] string telephoneNumber,
    [Required]string addresss,
    [Required] string productName,
    [Required] int purchaseQuantity,
    [Required,DataType(DataType.Currency)] decimal unitPrice,
    [Required,DataType(DataType.Currency)] decimal totalPrice,
    [Required] DateTime orderedDate
    );
}
