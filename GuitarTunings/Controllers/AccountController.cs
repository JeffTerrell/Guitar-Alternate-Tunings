using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GuitarTunings.Models;
using System.Threading.Tasks;
using GuitarTunings.ViewModels;
using System.Linq;

namespace GuitarTunings.Controllers
{
public class AccountController : Controller
{
    private readonly GuitarTuningsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, GuitarTuningsContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
    }

    public ActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }

    public ActionResult LogOff()
    {
      return View();
    }

    [HttpPost, ActionName("LogOff")]
    public async Task<ActionResult> LogOffConfirmed()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("LogOff");
    }

    public async Task<ActionResult> Edit(string name)
    {
    var user = await _userManager.FindByNameAsync(name);

    if (user == null)
    {
        ViewBag.ErrorMessage = $"User with Id = {"Jeff"} cannot be found";
        return View("NotFound");  // setup "NotFound" view in Shared
    }

    // GetClaimsAsync returns the list of user Claims
    var userClaims = await _userManager.GetClaimsAsync(user);
    // GetRolesAsync returns the list of user Roles
    var userRoles = await _userManager.GetRolesAsync(user);

    var model = new EditViewModel
    {
        Id = user.Id,
        Email = user.Email,
        UserName = user.UserName,
        Claims = userClaims.Select(c => c.Value).ToList(),
        Roles = (System.Collections.Generic.List<string>)userRoles
    };

    return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(EditViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
            return View("NotFound");  // setup "NotFound" view in Shared
        }
        else
        {
            user.Email = model.Email;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return await(LogOffConfirmed());
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
  }
}







// var id = _userManager.FindByNameAsync(name)