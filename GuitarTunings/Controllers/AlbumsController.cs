using System.Linq;
using GuitarTunings.Models;
using GuitarTunings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Controllers
{

  [Authorize]
  public class AlbumsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public AlbumsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    [AllowAnonymous]
    public ActionResult Index(int Id, string selectedLetter)
    {
      ViewBag.AlbumId = Id;
      var model = new AlphabetPagingViewModel<Album> {  SelectedLetter = selectedLetter };

      model.FirstLetters = _db.Albums
          .GroupBy(p => p.Name.Substring(0, 1))
          .Select(x => x.Key.ToUpper())
          .ToList();

      if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
      {
        model.GenericList = _db.Albums.ToList();  
      }
      else
      {
        if (selectedLetter == "0-9")
        {
          var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
          model.GenericList = _db.Albums.Where(p => numbers.Contains(p.Name.Substring(0, 1))).Select(p => p).ToList();
        }
        else
        {
          model.GenericList = _db.Albums.Where(p => p.Name.StartsWith(selectedLetter)).Select(p => p).ToList();
        }
      }
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create (Album album, int ArtistId, int SongId)
    {
      Album existingAlbum = _db.Albums.FirstOrDefault(x => x.Name == album.Name);
      if (existingAlbum != null)
      {
        TempData["AlbumDuplicate"] = ($"Cannot create {album.Name}, album already exists");
        ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
        ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");
        TempData["ExistingAlbumId"] = existingAlbum.AlbumId;
        return View();
      }

      if(album != null)
      {
        TempData["AlbumCreate"] = ($"Album {album.Name} successfully created");
        _db.Albums.Add(album);
        _db.SaveChanges();
      }  

      if(ArtistId != 0)
      {
        _db.AlbumArtists.Add(new AlbumArtist() { AlbumId = album.AlbumId, ArtistId = ArtistId});
        _db.SaveChanges();
      }

      if(SongId != 0)
      {
        _db.AlbumSongs.Add(new AlbumSong() { AlbumId = album.AlbumId, SongId = SongId});
        _db.SaveChanges();
      }

      return RedirectToAction("Index", new {id = album.AlbumId});
    }

    [AllowAnonymous]
    public ActionResult Details(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == Id);

      if (thisAlbum == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }
      return View(thisAlbum);
    }

    public ActionResult Edit(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == Id);

      if (thisAlbum == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");
      return View(thisAlbum);
    }

    [HttpPost]
    public ActionResult Edit(Album album, int AlbumId)
    {
      Album existingAlbum = _db.Albums.FirstOrDefault(x => x.Name == album.Name);
      if (existingAlbum != null)
      {
        ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
        ViewBag.SongId = new SelectList(_db.Songs, "SongId", "Name");
        TempData["AlbumDuplicate"] = ($"Cannot update {album.Name}, album already exists");
        TempData["ExistingAlbumId"] = existingAlbum.AlbumId;
        return View(existingAlbum);
      }

      if(album != null)
      {
        TempData["AlbumUpdate"] = ($"{album.Name} updated successfully!") ;
        _db.Entry(album).State = EntityState.Modified;
        _db.SaveChanges();
      }  
      return RedirectToAction("Details", new { id = album.AlbumId});
    }

    [HttpPost]
    public ActionResult AddArtist(Album album, int ArtistId)
    {
      AlbumArtist joinEntry = _db.AlbumArtists.Where(x => x.AlbumId == album.AlbumId).FirstOrDefault(y => y.ArtistId == ArtistId);

      if(joinEntry == null)
      {
        _db.AlbumArtists.Add(new AlbumArtist() { ArtistId = ArtistId , AlbumId = album.AlbumId});
        _db.SaveChanges();
        Artist artist = _db.Artists.FirstOrDefault(find => find.ArtistId == ArtistId);
        TempData["ArtistAdded"] = ($"\u00A0{artist.Name} added");
        return RedirectToAction("Edit", new { id = album.AlbumId });
      }

      if(joinEntry != null)
      {
        TempData["ArtistDuplicate"] = ($"\u00A0Cannot add {joinEntry.Artist.Name}, artist already exists");
        return RedirectToAction("Edit", new { id = album.AlbumId });
      }
      return RedirectToAction("Edit", new { id = album.AlbumId });
    }

    [HttpPost]
    public ActionResult DeleteArtist(Album album, int joinId)
    {
      AlbumArtist joinEntry = _db.AlbumArtists.FirstOrDefault(find => find.AlbumArtistId == joinId);
      _db.AlbumArtists.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = album.AlbumId });
    }

    [HttpPost]
    public ActionResult AddSong(Album album, int SongId)
    {
      AlbumSong joinEntry = _db.AlbumSongs.Where(x => x.AlbumId == album.AlbumId).FirstOrDefault(find => find.SongId == SongId);
      if(joinEntry == null)
      {
        _db.AlbumSongs.Add(new AlbumSong() { SongId = SongId , AlbumId = album.AlbumId});
        _db.SaveChanges();
        Song song = _db.Songs.FirstOrDefault(find => find.SongId == SongId);
        TempData["SongAdded"] = ($"\u00A0{song.Name} added");
        return Redirect(Url.Action("Edit", new { id = album.AlbumId }) + "#Songs");
      }

      if(joinEntry != null)
        {
          TempData["SongDuplicate"] = ($"\u00A0Cannot add {joinEntry.Song.Name}, song already exists");
          return RedirectToAction("Edit", new { id = album.AlbumId });
        }
      return RedirectToAction("Edit", new { id = album.AlbumId });
    }

    [HttpPost]
    public ActionResult DeleteSong(Album album, int joinId)
    {
      AlbumSong joinEntry = _db.AlbumSongs.FirstOrDefault(find => find.AlbumSongId == joinId);
      _db.AlbumSongs.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = album.AlbumId });
    }

    public ActionResult Delete(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == Id);

      if (thisAlbum == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      return View(thisAlbum);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int Id)
    {
      if(Id != 0)
      {
      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == Id);
      TempData["AlbumDelete"] = ($"{thisAlbum.Name} deleted successfully!") ;
      _db.Albums.Remove(thisAlbum);
      _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
  }
}