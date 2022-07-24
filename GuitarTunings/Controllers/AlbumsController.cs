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
    public ActionResult Index(int Id)
    {
      ViewBag.AlbumId = Id;
      return View(_db.Albums.ToList());
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
      if(album != null & ArtistId != 0 & SongId != 0)
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
    public ActionResult Edit(Album album)
    {
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
      _db.AlbumArtists.Add(new AlbumArtist() { ArtistId = ArtistId , AlbumId = album.AlbumId});
      _db.SaveChanges();
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
      _db.AlbumSongs.Add(new AlbumSong() { SongId = SongId , AlbumId = album.AlbumId});
      _db.SaveChanges();
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
      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == Id);
      _db.Albums.Remove(thisAlbum);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}