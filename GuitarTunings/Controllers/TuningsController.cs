using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Controllers
{

  public class TuningsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public TuningsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    public ActionResult Index()
    {
      return View(_db.Tunings.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.TuningCategoryId = new SelectList(_db.TuningCategories, "TuningCategoryId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create (Tuning tuning) // [Bind("ImageId, Name, Notes, Description, TuningCategoryId, ImageFileA")] 
    {
      // string wwwRootPath = _hostEnvironment.WebRootPath;
      // string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileA.FileName);
      // string extension = Path.GetExtension(tuning.ImageFileA.FileName);
      // tuning.ImageNameA= fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
      // string path = Path.Combine(wwwRootPath + "/Image/", fileName);
      // using (var fileStream = new FileStream(path, FileMode.Create))
      // {
      //   await tuning.ImageFileA.CopyToAsync(fileStream);
      // }

      _db.Tunings.Add(tuning);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int Id)
    {
      Tuning thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);
      return View(thisTuning);
    }

    public ActionResult Edit(int Id)
    {
      var thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);
      return View(thisTuning);
    }

    [HttpPost]
    public ActionResult Edit(Tuning tuning)
    {
      _db.Entry(tuning).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = tuning.TuningId});
    }

    public ActionResult Delete(int Id)
    {
      var thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);
      return View(thisTuning);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int Id)
    {
      var thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);
      _db.Tunings.Remove(thisTuning);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
