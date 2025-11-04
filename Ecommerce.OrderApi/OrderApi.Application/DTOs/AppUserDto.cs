
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record AppUserDto
    (
        int id,
        [Required]string name,
        [Required]string addresss,
        [Required]string telephoneNumber,
        [Required,EmailAddress] string email,
        [Required]string password,
        [Required] string role
    );
}
