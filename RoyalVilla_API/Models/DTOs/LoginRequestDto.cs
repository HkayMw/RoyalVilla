using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
