using System.ComponentModel.DataAnnotations;

namespace CI.API.ViewModels
{
  public class LoginViewmodel
  {
      [Required]
      public string Username { get; set; }
      [Required]
      public string Password { get; set; }
  }

}