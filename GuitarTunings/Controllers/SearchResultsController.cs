using System.Linq;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GuitarTunings.Controllers
{
  
  [Authorize]
  public class SearchResultsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SearchResultsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }
    [AllowAnonymous]
    public ActionResult Index(string model, string searchText = "")
    {
      if (model == "TuningCategory")
          {
            ViewBag.resultsTuningCategories = _db.TuningCategories.Where(result => result.Name.Contains(searchText)).ToList();
          }
      if (model == "Tuning")
          {
            ViewBag.resultsTunings = _db.Tunings.Where(result => result.Name.Contains(searchText)).ToList();
          }
      if (model == "Artist")
          {
            ViewBag.resultsArtists = _db.Artists.Where(result => result.Name.Contains(searchText)).ToList();
          }

      if (model == "Song")
          {
            ViewBag.resultsSongs = _db.Songs.Where(result => result.Name.Contains(searchText)).ToList();
          }
  
      return View();
    }
  }  
}