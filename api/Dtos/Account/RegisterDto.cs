using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }
    
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    
        [Required]
        public string? Password { get; set; }



    }
}