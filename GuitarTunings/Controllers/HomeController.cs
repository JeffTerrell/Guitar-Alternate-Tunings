using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GuitarTunings.Controllers
{

  [Authorize]
  public class HomeController : Controller
  {
    private readonly GuitarTuningsContext _db;

    public HomeController(GuitarTuningsContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    [HttpGet("/")]
    public ActionResult Index()
    {
      var listArtists = _db.Artists.ToList();
      var listSongs = _db.Songs.ToList();
      var listTunings = _db.Tunings.ToList();

      ViewBag.artistCount = listArtists.Count;
      ViewBag.songCount = listSongs.Count;
      ViewBag.tuningCount = listTunings.Count;

      return View(_db.Songs.OrderByDescending(song => song.SongId).Take(5).ToList());
    }
  }
}



// var songTest = _db.Songs.OrderByDescending(song => song.Name).Take(1).ToString();
// var songTest2 = _db.Songs.FirstOrDefault(song => song.Name == "teatsdf");
// ViewBag.LastUpdated = "Last Updated: " + System.IO.File.GetLastWriteTime(songTest2.Name);
// ViewBag.aSong = songTest2.Name;
// ViewBag.LastUpdated = "Last Updated: " + System.IO.File.GetLastWriteTime(_db.Songs.ToString());