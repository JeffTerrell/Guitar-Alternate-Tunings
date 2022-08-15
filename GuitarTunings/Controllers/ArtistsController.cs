using System;
using System.IO;
using System.Linq;
using GuitarTunings.Models;
using GuitarTunings.ViewModels;
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
    public ActionResult Index(int Id, string selectedLetter)
    {
      ViewBag.ArtistId = Id;
      var model = new AlphabetPagingViewModel<Artist> {  SelectedLetter = selectedLetter };

      model.FirstLetters = _db.Artists
          .GroupBy(p => p.Name.Substring(0, 1))
          .Select(x => x.Key.ToUpper())
          .ToList();

      if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
      {
        model.GenericList = _db.Artists.ToList();    
      }
      else
      {
        if (selectedLetter == "0-9")
        {
          var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
          model.GenericList = _db.Artists.Where(p => numbers.Contains(p.Name.Substring(0, 1))).Select(p => p).ToList();
        }   
        else
        {
          model.GenericList = _db.Artists.Where(p => p.Name.StartsWith(selectedLetter)).Select(p => p).ToList();  
        }
      }
      return View(model);
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult Index(string searchText = "")
    {
      return View(_db.Artists.Where(artist => artist.Name.StartsWith(searchText)).OrderBy(artist => artist.Name).ToList());
    }

    public ActionResult Create()
    {
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create([Bind("ImageId, Name, Genre, Description, ArtistImageFile")] Artist artist, int TuningId)
    {
      Artist existingArtist = _db.Artists.FirstOrDefault(x => x.Name == artist.Name);
      if (existingArtist != null)
      {
        TempData["ArtistDuplicate"] = ($"Cannot create {artist.Name}, artist already exists");
        ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
        TempData["ExistingArtistId"] = existingArtist.ArtistId;
        return View();
      }

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

      ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");  
      return View(thisArtist);
    }

    [HttpPost]
    public ActionResult Edit(Artist artist, string ArtistImageName)
    {
      Artist existingArtist = _db.Artists.FirstOrDefault(x => x.Name == artist.Name);
      if(existingArtist != null)
      {
        ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
        ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");
        ViewBag.TuningId = new SelectList(_db.Artists, "ArtistId", "Name");
        TempData["ArtistDuplicate"] = ($"Cannot update {artist.Name}, artist already exists");
        TempData["ExistingArtistId"] = existingArtist.ArtistId;
        return View(existingArtist);
      }

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
    public ActionResult AddAlbum(Artist artist, int AlbumId)
    {
      AlbumArtist joinEntry = _db.AlbumArtists.Where(x => x.ArtistId == artist.ArtistId).FirstOrDefault(y => y.AlbumId == AlbumId);

      if(joinEntry == null)
      {
        _db.AlbumArtists.Add(new AlbumArtist() { AlbumId = AlbumId, ArtistId = artist.ArtistId});
        _db.SaveChanges();
        Album album = _db.Albums.FirstOrDefault(find => find.AlbumId == AlbumId);
        TempData["AlbumAdded"] = ($"\u00A0{album.Name} added");
        return RedirectToAction("Edit", new { id = artist.ArtistId });
      }

      if(joinEntry != null)
      {
        TempData["AlbumDuplicate"] = ($"\u00A0Cannot add {joinEntry.Album.Name}, album already exists");
        return RedirectToAction("Edit", new { id = artist.ArtistId });
      }
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    [HttpPost]
    public ActionResult DeleteAlbum(Artist artist, int joinId)
    {
      AlbumArtist joinEntry = _db.AlbumArtists.FirstOrDefault(find => find.AlbumArtistId == joinId);
      _db.AlbumArtists.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = artist.ArtistId });
    }

    [HttpPost]
    public ActionResult AddSong(Artist artist, int SongId)
    {
      ArtistSong joinEntry = _db.ArtistSongs.Where(x => x.ArtistId == artist.ArtistId).FirstOrDefault(y => y.SongId == SongId);

      if(joinEntry == null)
      {
        _db.ArtistSongs.Add(new ArtistSong() { SongId = SongId, ArtistId = artist.ArtistId});
        _db.SaveChanges();
        Song song = _db.Songs.FirstOrDefault(find => find.SongId == SongId);
        TempData["SongAdded"] = ($"\u00A0{song.Name} added");
        return RedirectToAction("Edit", new { id = artist.ArtistId });
      }

      if(joinEntry != null)
      {
        TempData["SongDuplicate"] = ($"\u00A0Cannot add {joinEntry.Song.Name}, song already exists");
        return RedirectToAction("Edit", new { id = artist.ArtistId });
      }
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
      ArtistTuning joinEntry = _db.ArtistTunings.Where(x => x.ArtistId == artist.ArtistId).FirstOrDefault(y => y.TuningId == TuningId);

      if(joinEntry == null)
      {
        _db.ArtistTunings.Add(new ArtistTuning() { TuningId = TuningId, ArtistId = artist.ArtistId});
        _db.SaveChanges();
        Tuning tuning = _db.Tunings.FirstOrDefault(find => find.TuningId == TuningId);
        TempData["TuningAdded"] = ($"\u00A0{tuning.Name} added");
        return RedirectToAction("Edit", new { id = artist.ArtistId });
      }

      if(joinEntry != null)
      {
        TempData["TuningDuplicate"] = ($"\u00A0Cannot add {joinEntry.Tuning.Name}, tuning already exists");
        return RedirectToAction("Edit", new { id = artist.ArtistId });
      }
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