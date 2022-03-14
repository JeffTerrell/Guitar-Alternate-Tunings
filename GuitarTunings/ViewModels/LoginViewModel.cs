using System.ComponentModel.DataAnnotations;

namespace GuitarTunings.ViewModels
{
  public class LoginViewModel
  {
    [Required]
    public string UserName {get; set;}
    [Required]
    [DataType(DataType.Password)]
    public string Password {get; set;}
  }
}