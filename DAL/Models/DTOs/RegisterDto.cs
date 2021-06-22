using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Password { get; set; }
    }
}