using System.Linq;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Controllers
{

  [Authorize]
  public class TuningCategoriesController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public TuningCategoriesController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    public ActionResult Details(int Id)
    {
      TuningCategory thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);
      return View(thisTuningCategory);
    }

    public ActionResult Edit(int Id)
    {
      var thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);
      return View(thisTuningCategory);
    }

    [HttpPost]
    public ActionResult Edit(TuningCategory tuningCategory)
    {
      _db.Entry(tuningCategory).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = tuningCategory.TuningCategoryId});
    }

    public ActionResult Delete(int Id)
    {
      var thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);
      return View(thisTuningCategory);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int Id)
    {
      var thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);
      _db.TuningCategories.Remove(thisTuningCategory);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
