using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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
  public class ArtistsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ArtistsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    [AllowAnonymous]
    public ActionResult Index(int Id)
    {
      ViewBag.ArtistId = Id;
      return View(_db.Artists.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create([Bind("ImageId, Name, Genre, Description, ArtistImageFile")] Artist artist, int TuningId)
    {
      AddImage(artist);

      _db.Artists.Add(artist);
      _db.SaveChanges();

      if (TuningId != 0)
      {
        TempData["ArtistCreate"] = ($"Artist {artist.Name} succesfully created");
        _db.ArtistTunings.Add(new ArtistTuning() { ArtistId = artist.ArtistId, TuningId = TuningId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index", new {id = artist.ArtistId});
    }

    [AllowAnonymous]
    public ActionResult Details(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);

      if (thisArtist == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }
      return View(thisArtist);
    }

    public ActionResult Edit(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);

      if (thisArtist == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");     
      return View(thisArtist);
    }

    [HttpPost]
    public ActionResult Edit(Artist artist, string ArtistImageName)
    {
      if (ArtistImageName != null)
      {
        DeleteImage(ArtistImageName);
      }

      if(artist != null)
      {
        TempData["ArtistUpdate"] = ($"{artist.Name} updated successfully!");
        AddImage(artist);
        _db.Entry(artist).State = EntityState.Modified;
        _db.SaveChanges();     
      }
      return RedirectToAction("Details", new { id = artist.ArtistId});
    }

    [HttpPost]
    public ActionResult AddSong(Artist artist, int SongId)
    {
      // Tuning thisTuning = _db.Tunings.FirstOrDefault(find => find.TuningId == TuningId);

      _db.ArtistSongs.Add(new ArtistSong() { SongId = SongId , ArtistId = artist.ArtistId});
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    [HttpPost]
    public ActionResult DeleteSong(Artist artist, int joinId)
    {
      ArtistSong joinEntry = _db.ArtistSongs.FirstOrDefault(find => find.ArtistSongId == joinId);
      _db.ArtistSongs.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    [HttpPost]
    public ActionResult AddTuning(Artist artist, int TuningId)
    {
      // Tuning thisTuning = _db.Tunings.FirstOrDefault(find => find.TuningId == TuningId);

      _db.ArtistTunings.Add(new ArtistTuning() { TuningId = TuningId , ArtistId = artist.ArtistId});
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    [HttpPost]
    public ActionResult DeleteTuning(Artist artist, int joinId)
    {
      ArtistTuning joinEntry = _db.ArtistTunings.FirstOrDefault(find => find.ArtistTuningId == joinId);
      _db.ArtistTunings.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    public ActionResult Delete(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Artist thisArtist = _db.Artists.FirstOrDefault(artist => artist.ArtistId == Id);

      if (thisArtist == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }
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

    [NonAction]
    public async void AddImage (Artist artist)
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
    }

    [NonAction]
    public void DeleteImage(string imageName)
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