using System.Collections.Generic;
using System.Linq;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Controllers
{

  public class SearchResultsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SearchResultsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    public ActionResult Index(string searchText = "")
    {
      List<TuningCategory> results;

      if (searchText != "" && searchText != null)
      {
        results = _db.TuningCategories.Where(result => result.Name.Contains(searchText)).ToList();
      }
      else
        results = _db.TuningCategories.ToList();

      return View(results);
    }
  }  
}