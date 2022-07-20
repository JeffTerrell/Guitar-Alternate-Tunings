using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Controllers
{

  [Authorize]
  public class TuningsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public TuningsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    [AllowAnonymous]
    public ActionResult Index(int Id)
    {
      ViewBag.TuningId = Id;
      return View(_db.Tunings.OrderBy(tuning => tuning.Name).ToList());
    }

    public ActionResult Create()
    {
      ViewBag.TuningCategoryId = new SelectList(_db.TuningCategories, "TuningCategoryId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create ([Bind("Name, Notes, Description, TuningCategoryId, ImageFileA, ImageFileB, ImageFileC, ImageFileD, ImageFileE, ImageFileF, ImageFileG")] Tuning tuning)
    {

      AddImage(tuning);
      TempData["TuningCreate"] = ($"Tuning {tuning.Name} successfully created");
      _db.Tunings.Add(tuning);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index", new {id = tuning.TuningId});
    }

    [AllowAnonymous]
    public ActionResult Details(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Tuning thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);

      if (thisTuning == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      return View(thisTuning);
    }

    public ActionResult Edit(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Tuning thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);

      if (thisTuning == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      ViewBag.TuningCategoryId = new SelectList(_db.TuningCategories, "TuningCategoryId", "Name");
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      return View(thisTuning);
    }

    [HttpPost]
    public async Task<ActionResult> Edit (Tuning tuning, string ImageNameA, string ImageNameB, string ImageNameC, string ImageNameD, string ImageNameE, string ImageNameF, string ImageNameG)
    {      

      if((ImageNameA != null) || (ImageNameB != null) || (ImageNameC != null) || (ImageNameD != null) || (ImageNameE != null) || (ImageNameF != null) || (ImageNameG != null))
      {
        DeleteImage(ImageNameA);
        DeleteImage(ImageNameB);
        DeleteImage(ImageNameC);
        DeleteImage(ImageNameD);
        DeleteImage(ImageNameE);
        DeleteImage(ImageNameF);
        DeleteImage(ImageNameG);
      }

      if(tuning != null)
      {
        TempData["TuningUpdate"] = ($"{tuning.Name} updated successfully!");
        AddImage(tuning);
        _db.Entry(tuning).State = EntityState.Modified;
        await _db.SaveChangesAsync();
      }

      return RedirectToAction("Details", new { id = tuning.TuningId});
    }

    [HttpPost]
    public ActionResult AddArtist(Tuning tuning, int ArtistId)
    {
      _db.ArtistTunings.Add(new ArtistTuning() { ArtistId = ArtistId , TuningId = tuning.TuningId});
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = tuning.TuningId });
    }

    [HttpPost]
    public ActionResult DeleteArtist(Tuning tuning, int joinId)
    {
      ArtistTuning joinEntry = _db.ArtistTunings.FirstOrDefault(find => find.ArtistTuningId == joinId);
      _db.ArtistTunings.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = tuning.TuningId });
    }

    public ActionResult Delete(int Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Tuning thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);

      if (thisTuning == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }      

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

    [NonAction]
    public async void AddImage (Tuning tuning)
    {
      if(tuning.ImageFileA != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileA.FileName);
        string extension = Path.GetExtension(tuning.ImageFileA.FileName);
        tuning.ImageNameA = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileA.CopyToAsync(fileStream);
        }
      }

      if(tuning.ImageFileB != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileB.FileName);
        string extension = Path.GetExtension(tuning.ImageFileB.FileName);
        tuning.ImageNameB = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileB.CopyToAsync(fileStream);
        }
      }

      if(tuning.ImageFileC != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileC.FileName);
        string extension = Path.GetExtension(tuning.ImageFileA.FileName);
        tuning.ImageNameC = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileC.CopyToAsync(fileStream);
        }
      }

      if(tuning.ImageFileD != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileD.FileName);
        string extension = Path.GetExtension(tuning.ImageFileD.FileName);
        tuning.ImageNameD = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileD.CopyToAsync(fileStream);
        }
      }

      if(tuning.ImageFileE != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileE.FileName);
        string extension = Path.GetExtension(tuning.ImageFileE.FileName);
        tuning.ImageNameE = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileE.CopyToAsync(fileStream);
        }
      }

      if(tuning.ImageFileF != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileF.FileName);
        string extension = Path.GetExtension(tuning.ImageFileF.FileName);
        tuning.ImageNameF = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileF.CopyToAsync(fileStream);
        }
      }

      if(tuning.ImageFileG != null)
      {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(tuning.ImageFileG.FileName);
        string extension = Path.GetExtension(tuning.ImageFileG.FileName);
        tuning.ImageNameG = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await tuning.ImageFileG.CopyToAsync(fileStream);
        }
      }                                      
    }

    [NonAction]
    public void DeleteImage(string imageName)
    {
      if(imageName != null)
      {
        string wwwRootPath = Path.Combine(_hostEnvironment.WebRootPath, "Image");
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(),wwwRootPath, imageName);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
      }
    }
  }
}