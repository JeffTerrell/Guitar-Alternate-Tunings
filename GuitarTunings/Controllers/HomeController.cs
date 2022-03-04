using Microsoft.AspNetCore.Mvc;

namespace GuitarTunings.Controllers
{

  public class HomeController : Controller
  {

    [HttpGet("/")]
    public IActionResult Index()
    {
      return View();
    }
  }
}