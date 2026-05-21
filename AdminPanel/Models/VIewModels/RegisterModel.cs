using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.VIewModels;

public class RegisterModel
{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string FullName { get; set; }
}