using System.ComponentModel.DataAnnotations;

namespace CI.API.ViewModels
{
  public class ConfirmEmailViewmodel 
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}