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
      var model = new AlphabetPagingViewModel {  SelectedLetter = selectedLetter };

      model.FirstLetters = _db.Songs
          .GroupBy(p => p.Name.Substring(0, 1))
          .Select(x => x.Key.ToUpper())
          .ToList();

      if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
      {
        model.Names = _db.Songs
            .Select(p => p.Name)
            .ToList();
        model.IDs = _db.Songs.Select(p => p.SongId).ToList(); 
        model.Dict = Enumerable.Range(0, model.IDs.Count).ToDictionary(i => model.IDs[i], i=> model.Names[i]);    
      }
      else
      {
        if (selectedLetter == "0-9")
        {
          var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
          model.Names = _db.Songs
              .Where(p => numbers.Contains(p.Name.Substring(0, 1)))
              .Select(p => p.Name)
              .ToList();
          model.IDs = _db.Songs
          .Where(p => numbers.Contains(p.Name.Substring(0, 1)))
          .Select(p => p.SongId)
          .ToList();
          model.Dict = Enumerable.Range(0, model.IDs.Count).ToDictionary(i => model.IDs[i], i=> model.Names[i]);     
        }
        else
        {
          model.Names = _db.Songs
              .Where(p => p.Name.StartsWith(selectedLetter))
              .Select(p => p.Name)
              .ToList();
          model.IDs = _db.Songs
              .Where(p => p.Name.StartsWith(selectedLetter))
              .Select(p => p.SongId)
              .ToList();
          model.Dict = Enumerable.Range(0, model.IDs.Count).ToDictionary(i => model.IDs[i], i=> model.Names[i]);     
        }
      }
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create (Song song)
    {
      if(song.TuningId == 0)
      {
        ViewBag.Message = "Please choose an Alternate Tuning";
        ViewBag.TuningId = new SelectList(_db.Tunings, "TuningId", "Name");
        return View();
      }

      if(song != null)
      {
        TempData["SongCreate"] = ($"Song {song.Name} successfully created");
        _db.Songs.Add(song);
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
      _db.ArtistSongs.Add(new ArtistSong() { ArtistId = ArtistId , SongId = song.SongId});
      _db.SaveChanges();
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