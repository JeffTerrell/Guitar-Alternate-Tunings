using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GuitarTunings.ViewModels
{
  
  public class EditViewModel
  {

    public EditViewModel()
    {
      Claims = new List<string>();
      Roles = new List<string>();
    }

    public string Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Password does not match.")]
    public string ConfirmPassword {get; set;}

    public List<string> Claims { get; set; }
    public List<string> Roles { get; set; }

  }
}