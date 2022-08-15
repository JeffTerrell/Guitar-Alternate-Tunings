using System;
using System.Linq;
using GuitarTunings.Models;
using GuitarTunings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult Index(int Id, string selectedLetter)
    {
      ViewBag.TuningCategoryId = Id;
      var model = new AlphabetPagingViewModel<TuningCategory> {  SelectedLetter = selectedLetter };

      model.FirstLetters = _db.TuningCategories
          .GroupBy(p => p.Name.Substring(0, 1))
          .Select(x => x.Key.ToUpper())
          .ToList();

      if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
      { 
        model.GenericList = _db.TuningCategories.ToList();
      }
      else
      {
        if (selectedLetter == "0-9")
        {
          var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
          model.GenericList = _db.TuningCategories.Where(p => numbers.Contains(p.Name.Substring(0, 1))).Select(p => p).ToList();     
        }
        else
        {
          model.GenericList = _db.TuningCategories.Where(p => p.Name.StartsWith(selectedLetter)).Select(p => p).ToList(); 
        }
      }
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create (TuningCategory tuningCategory)
    {
      if(tuningCategory != null)
      {
        TempData["TuningCategoryCreate"] = ($"Tuning category {tuningCategory.Name} successfully created");
        _db.TuningCategories.Add(tuningCategory);
        _db.SaveChanges();
      }  
      return RedirectToAction("Index", new {id = tuningCategory.TuningCategoryId});
    }

    [AllowAnonymous]
    public ActionResult Details(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      TuningCategory thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);

      if (thisTuningCategory == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      return View(thisTuningCategory);
    }

    public ActionResult Edit(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      TuningCategory thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);

      if (thisTuningCategory == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      return View(thisTuningCategory);
    }

    [HttpPost]
    public ActionResult Edit(TuningCategory tuningCategory)
    {
      if(tuningCategory != null)
      {
        TempData["TuningCategoryUpdate"] = ($"{tuningCategory.Name} updated successfully!");
        _db.Entry(tuningCategory).State = EntityState.Modified;
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = tuningCategory.TuningCategoryId});
    }

    public ActionResult Delete(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      TuningCategory thisTuningCategory = _db.TuningCategories.FirstOrDefault(tuningCategory => tuningCategory.TuningCategoryId == Id);

      if (thisTuningCategory == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

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
