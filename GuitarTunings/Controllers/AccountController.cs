using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GuitarTunings.Models;
using System.Threading.Tasks;
using GuitarTunings.ViewModels;
using System.Linq;

namespace GuitarTunings.Controllers
{
[Authorize]
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

    [AllowAnonymous]
    public ActionResult Login()
    {
      return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        TempData["LoginSuccess"] = ($"{model.UserName} successfully logged in!");
        return RedirectToAction("Index");
      }
      else
      {
        TempData["UserNotFound"] = "User not found!";
        return View();
      }
    }

    [AllowAnonymous]
    public ActionResult LogOff()
    {
      return View();
    }

    [HttpPost, ActionName("LogOff")]
    public async Task<ActionResult> LogOffConfirmed()
    {
      TempData["AccountLogOff"] = ($"{User.Identity.Name} successfully logged out");
      await _signInManager.SignOutAsync();
      return RedirectToAction("LogOff");
    }

    public async Task<ActionResult> Edit(string name)
    {

    if (name == null)
    {
      TempData["userNotFound"] = "null";
      return RedirectToAction("Index");
    }

    var user = await _userManager.FindByNameAsync(name);

    if (user == null)
    {
      TempData["userNotFound"] = $"{name}";
      return RedirectToAction("Index");
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
        TempData["userNotFound"] = $"User {model.UserName} cannot be found";
        return RedirectToAction("Index", "NotFound");
      }
      else
      {
        user.Email = model.Email;
        user.UserName = model.UserName;
        user.PasswordHash = model.Password;

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