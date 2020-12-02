using System.ComponentModel.DataAnnotations;

namespace CI.API.ViewModels
{
    public class CreateEmployerViewmodel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}