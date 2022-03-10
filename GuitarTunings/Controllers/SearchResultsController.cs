using System;
using System.Collections.Generic;
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
  public class SearchResultsController : Controller
  {
    private readonly GuitarTuningsContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SearchResultsController(GuitarTuningsContext db, IWebHostEnvironment hostEnvironment)
    {
      _db = db;
      this._hostEnvironment = hostEnvironment;
    }
    [AllowAnonymous]
    public ActionResult Index(string model, string searchText = "")
    {

      if (model == "TuningCategory")
        {

        if (searchText != "" && searchText != null)
        {
          ViewBag.resultsTuningCategories = _db.TuningCategories.Where(result => result.Name.Contains(searchText)).ToList();
        }
        else
          ViewBag.resultsTuningCategories = _db.TuningCategories.Where(result => result.Name.Contains(searchText)).ToList();
        }
    if (model == "Tuning")
        {

        if (searchText != "" && searchText != null)
        {
          ViewBag.resultsTunings = _db.Tunings.Where(result => result.Name.Contains(searchText)).ToList();
        }
        else
          ViewBag.resultsTunings = _db.Tunings.Where(result => result.Name.Contains(searchText)).ToList();
        }

    if (model == "Artist")
        {

        if (searchText != "" && searchText != null)
        {
          ViewBag.resultsArtists = _db.Artists.Where(result => result.Name.Contains(searchText)).ToList();
        }
        else
          ViewBag.resultsArtists = _db.Artists.Where(result => result.Name.Contains(searchText)).ToList();
        }

    if (model == "Song")
        {

        if (searchText != "" && searchText != null)
        {
          ViewBag.resultsSongs = _db.Songs.Where(result => result.Name.Contains(searchText)).ToList();
        }
        else
          ViewBag.resultsSongs = _db.Songs.Where(result => result.Name.Contains(searchText)).ToList();
        }          
  
      return View();
    }
  }  
}