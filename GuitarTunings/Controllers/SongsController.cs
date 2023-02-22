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
  public class SongsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SongsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }

    [AllowAnonymous]
    public ActionResult Index(int Id, string selectedLetter)
    {
      ViewBag.SongId = Id;
      var model = new AlphabetPagingViewModel<Song> { SelectedLetter = selectedLetter };
      model.AddToListAllAndNumbers();

      model.FirstLetters = _db.Songs
          .GroupBy(p => p.Name.Substring(0, 1))
          .Select(x => x.Key.ToUpper())
          .ToList();

      if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
      {
        model.GenericList = _db.Songs.ToList();
      }
      else
      {
        if (selectedLetter == "0-9")
        {
          var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
          model.GenericList = _db.Songs.Where(p => numbers.Contains(p.Name.Substring(0, 1))).Select(p => p).ToList();
        }
        else
        {
          model.GenericList = _db.Songs.Where(p => p.Name.StartsWith(selectedLetter)).Select(p => p).ToList();
        }
      }
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create (Song song, int ArtistId)
    {
      if(song.TuningId == 0)
      {
        ViewBag.Message = "Please choose an Alternate Tuning";
        ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
        return View();
      }

      if(song != null)
      {
        _db.Songs.Add(song);
        _db.SaveChanges();
      }

      if(ArtistId != 0)
      {
        TempData["SongCreate"] = ($"Song {song.Name} successfully created");
        _db.ArtistSongs.Add(new ArtistSong() { SongId = song.SongId, ArtistId = ArtistId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index", new {id = song.SongId});
    }

    [AllowAnonymous]
    public ActionResult Details(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Song thisSong = _db.Songs.FirstOrDefault(song => song.SongId == Id);

      if (thisSong == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }
      return View(thisSong);
    }

    public ActionResult Edit(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Song thisSong = _db.Songs.FirstOrDefault(song => song.SongId == Id);

      if (thisSong == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      return View(thisSong);
    }

    [HttpPost]
    public ActionResult Edit(Song song)
    {
      if(song != null)
      {
        TempData["SongUpdate"] = ($"{song.Name} updated successfully!") ;
        _db.Entry(song).State = EntityState.Modified;
        _db.SaveChanges();
      }  
      return RedirectToAction("Details", new { id = song.SongId});
    }

    [HttpPost]
    public ActionResult AddArtist(Song song, int ArtistId)
    {
      ArtistSong joinEntry = _db.ArtistSongs.Where(x => x.SongId == song.SongId).FirstOrDefault(y => y.ArtistId == ArtistId);

      if(joinEntry == null)
      {
        _db.ArtistSongs.Add(new ArtistSong() { ArtistId = ArtistId , SongId = song.SongId});
        _db.SaveChanges();
        Artist artist = _db.Artists.FirstOrDefault(find => find.ArtistId == ArtistId);
        TempData["ArtistAdded"] = ($"\u00A0{artist.Name} added");
        return RedirectToAction("Edit", new { id = song.SongId });
      }

      if (joinEntry != null)
      {
        TempData["ArtistDuplicate"] = ($"\u00A0Cannot add {joinEntry.Artist.Name}, artist already exists");
        return RedirectToAction("Edit", new { id = song.SongId });
      }

      return RedirectToAction("Edit", new { id = song.SongId });
    }

    [HttpPost]
    public ActionResult DeleteArtist(Song song, int joinId)
    {
      ArtistSong joinEntry = _db.ArtistSongs.FirstOrDefault(find => find.ArtistSongId == joinId);
      _db.ArtistSongs.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = song.SongId });
    }

    public ActionResult Delete(int? Id)
    {
      if (Id == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      Song thisSong = _db.Songs.FirstOrDefault(song => song.SongId == Id);

      if (thisSong == null)
      {
        TempData["urlNotFound"] = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path);
        return RedirectToAction("Index");
      }

      return View(thisSong);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int Id)
    {
      Song thisSong = _db.Songs.FirstOrDefault(song => song.SongId == Id);
      _db.Songs.Remove(thisSong);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}