using System;
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

  public class ArtistsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ArtistsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    public ActionResult Index()
    {
      return View(_db.Artists.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create ([Bind("ImageId, Name, Genre, Description, ArtistImageFile")] Artist artist, int TuningId)
    {

        string wwwRootPath = _hostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(artist.ArtistImageFile.FileName);
        string extension = Path.GetExtension(artist.ArtistImageFile.FileName);
        artist.ArtistImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await artist.ArtistImageFile.CopyToAsync(fileStream);
        }

      _db.Artists.Add(artist);
      _db.SaveChanges();

      if (TuningId != 0)
      {
        _db.ArtistTunings.Add(new ArtistTuning() { ArtistId = artist.ArtistId, TuningId = TuningId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int Id)
    {
      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);
      return View(thisArtist);
    }

    public ActionResult Edit(int Id)
    {
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);
      return View(thisArtist);
    }

    [HttpPost]
    public ActionResult Edit(Artist artist)
    {
      _db.Entry(artist).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = artist.ArtistId});
    }

    [HttpPost]
    public ActionResult AddTuning(Artist artist, int TuningId)
    {
      // Tuning thisTuning = _db.Tunings.FirstOrDefault(find => find.TuningId == TuningId);

      _db.ArtistTunings.Add(new ArtistTuning() { TuningId = TuningId , ArtistId = artist.ArtistId});
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    public ActionResult Delete(int Id)
    {
      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);
      return View(thisArtist);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int Id)
    {
      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);
      _db.Artists.Remove(thisArtist);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}