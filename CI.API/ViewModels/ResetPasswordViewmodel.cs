using System.ComponentModel.DataAnnotations;

namespace CI.API.ViewModels
{
  public class ResetPasswordViewmodel
  {
    [Required]
    public string  Email { get; set; }
  }

}