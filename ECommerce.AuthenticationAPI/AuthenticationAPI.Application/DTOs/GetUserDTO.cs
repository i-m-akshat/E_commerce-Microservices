
using System.ComponentModel.DataAnnotations;


namespace AuthenticationAPI.Application.DTOs
{
    public  record GetUserDTO
    (
        int id,

        [Required] string name,

        [Required] string addresss,

        [Required] string telephoneNumber,

        [Required, EmailAddress] string email,

        [Required] string role
    );
}
