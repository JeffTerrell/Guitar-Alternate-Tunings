using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GuitarTunings.Controllers
{

  [Authorize]
  public class NotFoundController : Controller
  {
    private readonly GuitarTuningsContext _db;

    public NotFoundController(GuitarTuningsContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    [HttpGet("/NotFound")]
    public ActionResult Index()
    {
      return View();
    }
  }
}