using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AuthenticationAPI.Application.DTOs
{
        public record AppUserDTO
        (
            int id,
            [Required] string name,
            [Required] string addresss,
            [Required] string telephoneNumber,
            [Required, EmailAddress] string email,
            [Required] string password,
            [Required] string role
        );

    
}
