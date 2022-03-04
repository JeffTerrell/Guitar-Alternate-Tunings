using System.Linq;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GuitarTunings.Controllers
{

  public class TuningCategoriesController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public TuningCategoriesController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    public ActionResult Index()
    {
      return View(_db.TuningCategories.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create (TuningCategory tuningCategory)
    {
      _db.TuningCategories.Add(tuningCategory);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int Id)
    {
      TuningCategory thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);
      return View(thisTuningCategory);
    }
  }
}
