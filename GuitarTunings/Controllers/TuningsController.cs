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
    public async Task<ActionResult> Create ([Bind("ImageId, Name, Notes, Description, TuningCategoryId, ImageFileA, ImageFileB, ImageFileC, ImageFileD, ImageFileE, ImageFileF, ImageFileG")] Tuning tuning)
    {
      if ("ImageFileA"!= null || tuning.ImageFileB != null || tuning.ImageFileC != null || tuning.ImageFileD != null || tuning.ImageFileE != null || tuning.ImageFileF != null || tuning.ImageFileG != null)
      {
        string wwwRootPathA = _hostEnvironment.WebRootPath;
        string fileNameA = Path.GetFileNameWithoutExtension(tuning.ImageFileA.FileName);
        string extensionA = Path.GetExtension(tuning.ImageFileA.FileName);
        tuning.ImageNameA = fileNameA = fileNameA + DateTime.Now.ToString("yymmssfff") + extensionA;
        string pathA = Path.Combine(wwwRootPathA + "/Image/", fileNameA);
        using (var fileStream = new FileStream(pathA, FileMode.Create))
        {
          await tuning.ImageFileA.CopyToAsync(fileStream);
        }

        string wwwRootPathB = _hostEnvironment.WebRootPath;
        string fileNameB = Path.GetFileNameWithoutExtension(tuning.ImageFileB.FileName);
        string extensionB = Path.GetExtension(tuning.ImageFileB.FileName);
        tuning.ImageNameB = fileNameB = fileNameB + DateTime.Now.ToString("yymmssfff") + extensionB;
        string pathB = Path.Combine(wwwRootPathB + "/Image/", fileNameB);
        using (var fileStream = new FileStream(pathB, FileMode.Create))
        {
          await tuning.ImageFileB.CopyToAsync(fileStream);
        }

        string wwwRootPathC = _hostEnvironment.WebRootPath;
        string fileNameC = Path.GetFileNameWithoutExtension(tuning.ImageFileC.FileName);
        string extensionC = Path.GetExtension(tuning.ImageFileC.FileName);
        tuning.ImageNameC = fileNameC = fileNameC + DateTime.Now.ToString("yymmssfff") + extensionC;
        string pathC = Path.Combine(wwwRootPathC + "/Image/", fileNameC);
        using (var fileStream = new FileStream(pathC, FileMode.Create))
        {
          await tuning.ImageFileC.CopyToAsync(fileStream);
        }

        string wwwRootPathD = _hostEnvironment.WebRootPath;
        string fileNameD = Path.GetFileNameWithoutExtension(tuning.ImageFileD.FileName);
        string extensionD = Path.GetExtension(tuning.ImageFileD.FileName);
        tuning.ImageNameD = fileNameD = fileNameD + DateTime.Now.ToString("yymmssfff") + extensionD;
        string pathD = Path.Combine(wwwRootPathD + "/Image/", fileNameD);
        using (var fileStream = new FileStream(pathD, FileMode.Create))
        {
          await tuning.ImageFileD.CopyToAsync(fileStream);
        }

        string wwwRootPathE = _hostEnvironment.WebRootPath;
        string fileNameE = Path.GetFileNameWithoutExtension(tuning.ImageFileE.FileName);
        string extensionE = Path.GetExtension(tuning.ImageFileE.FileName);
        tuning.ImageNameE = fileNameE = fileNameE + DateTime.Now.ToString("yymmssfff") + extensionE;
        string pathE = Path.Combine(wwwRootPathE + "/Image/", fileNameE);
        using (var fileStream = new FileStream(pathE, FileMode.Create))
        {
          await tuning.ImageFileE.CopyToAsync(fileStream);
        }

        string wwwRootPathF = _hostEnvironment.WebRootPath;
        string fileNameF = Path.GetFileNameWithoutExtension(tuning.ImageFileF.FileName);
        string extensionF = Path.GetExtension(tuning.ImageFileB.FileName);
        tuning.ImageNameF = fileNameF = fileNameF + DateTime.Now.ToString("yymmssfff") + extensionF;
        string pathF = Path.Combine(wwwRootPathF + "/Image/", fileNameF);
        using (var fileStream = new FileStream(pathF, FileMode.Create))
        {
          await tuning.ImageFileF.CopyToAsync(fileStream);
        }

        string wwwRootPathG = _hostEnvironment.WebRootPath;
        string fileNameG = Path.GetFileNameWithoutExtension(tuning.ImageFileG.FileName);
        string extensionG = Path.GetExtension(tuning.ImageFileG.FileName);
        tuning.ImageNameG = fileNameG = fileNameG + DateTime.Now.ToString("yymmssfff") + extensionG;
        string pathG = Path.Combine(wwwRootPathG + "/Image/", fileNameG);
        using (var fileStream = new FileStream(pathG, FileMode.Create))
        {
          await tuning.ImageFileG.CopyToAsync(fileStream);
        }

        _db.Tunings.Add(tuning);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      else
      {
        return NotFound();
      }
    }

    public ActionResult Details(int Id)
    {
      Tuning thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);
      return View(thisTuning);
    }

    public ActionResult Edit(int Id)
    {
      ViewBag.TuningCategoryId = new SelectList(_db.TuningCategories, "TuningCategoryId", "Name");
      var thisTuning = _db.Tunings.FirstOrDefault(tuning => tuning.TuningId == Id);
      return View(thisTuning);
    }

    [HttpPost]
    public async Task<ActionResult> Edit ([Bind("ImageId, Name, Notes, Description, TuningCategoryId, ImageFileA, ImageFileB, ImageFileC, ImageFileD, ImageFileE, ImageFileF, ImageFileG")] Tuning tuning)
    {
  
      string wwwRootPathA = _hostEnvironment.WebRootPath;
      string fileNameA = Path.GetFileNameWithoutExtension(tuning.ImageFileA.FileName);
      string extensionA = Path.GetExtension(tuning.ImageFileA.FileName);
      tuning.ImageNameA = fileNameA = fileNameA + DateTime.Now.ToString("yymmssfff") + extensionA;
      string pathA = Path.Combine(wwwRootPathA + "/Image/", fileNameA);
      using (var fileStream = new FileStream(pathA, FileMode.Create))
      {
        await tuning.ImageFileA.CopyToAsync(fileStream);
      }
 
      string wwwRootPathB = _hostEnvironment.WebRootPath;
      string fileNameB = Path.GetFileNameWithoutExtension(tuning.ImageFileB.FileName);
      string extensionB = Path.GetExtension(tuning.ImageFileB.FileName);
      tuning.ImageNameB = fileNameB = fileNameB + DateTime.Now.ToString("yymmssfff") + extensionB;
      string pathB = Path.Combine(wwwRootPathB + "/Image/", fileNameB);
      using (var fileStream = new FileStream(pathB, FileMode.Create))
      {
        await tuning.ImageFileB.CopyToAsync(fileStream);
      }

      string wwwRootPathC = _hostEnvironment.WebRootPath;
      string fileNameC = Path.GetFileNameWithoutExtension(tuning.ImageFileC.FileName);
      string extensionC = Path.GetExtension(tuning.ImageFileC.FileName);
      tuning.ImageNameC = fileNameC = fileNameC + DateTime.Now.ToString("yymmssfff") + extensionC;
      string pathC = Path.Combine(wwwRootPathC + "/Image/", fileNameC);
      using (var fileStream = new FileStream(pathC, FileMode.Create))
      {
        await tuning.ImageFileC.CopyToAsync(fileStream);
      }

      string wwwRootPathD = _hostEnvironment.WebRootPath;
      string fileNameD = Path.GetFileNameWithoutExtension(tuning.ImageFileD.FileName);
      string extensionD = Path.GetExtension(tuning.ImageFileD.FileName);
      tuning.ImageNameD = fileNameD = fileNameD + DateTime.Now.ToString("yymmssfff") + extensionD;
      string pathD = Path.Combine(wwwRootPathD + "/Image/", fileNameD);
      using (var fileStream = new FileStream(pathD, FileMode.Create))
      {
        await tuning.ImageFileD.CopyToAsync(fileStream);
      }

      string wwwRootPathE = _hostEnvironment.WebRootPath;
      string fileNameE = Path.GetFileNameWithoutExtension(tuning.ImageFileE.FileName);
      string extensionE = Path.GetExtension(tuning.ImageFileE.FileName);
      tuning.ImageNameE = fileNameE = fileNameE + DateTime.Now.ToString("yymmssfff") + extensionE;
      string pathE = Path.Combine(wwwRootPathE + "/Image/", fileNameE);
      using (var fileStream = new FileStream(pathE, FileMode.Create))
      {
        await tuning.ImageFileE.CopyToAsync(fileStream);
      }

      string wwwRootPathF = _hostEnvironment.WebRootPath;
      string fileNameF = Path.GetFileNameWithoutExtension(tuning.ImageFileF.FileName);
      string extensionF = Path.GetExtension(tuning.ImageFileB.FileName);
      tuning.ImageNameF = fileNameF = fileNameF + DateTime.Now.ToString("yymmssfff") + extensionF;
      string pathF = Path.Combine(wwwRootPathF + "/Image/", fileNameF);
      using (var fileStream = new FileStream(pathF, FileMode.Create))
      {
        await tuning.ImageFileF.CopyToAsync(fileStream);
      }

      string wwwRootPathG = _hostEnvironment.WebRootPath;
      string fileNameG = Path.GetFileNameWithoutExtension(tuning.ImageFileG.FileName);
      string extensionG = Path.GetExtension(tuning.ImageFileG.FileName);
      tuning.ImageNameG = fileNameG = fileNameG + DateTime.Now.ToString("yymmssfff") + extensionG;
      string pathG = Path.Combine(wwwRootPathG + "/Image/", fileNameG);
      using (var fileStream = new FileStream(pathG, FileMode.Create))
      {
        await tuning.ImageFileG.CopyToAsync(fileStream);
      }
      
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
