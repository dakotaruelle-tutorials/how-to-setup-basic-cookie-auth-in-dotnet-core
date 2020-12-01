using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Login(string returnUrl = "")
    {
      return View("Login", returnUrl);
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, string returnUrl = "")
    {
      if (username == password)
      {
        var claims = new List<Claim>
        {
          new Claim("sub", username),
          new Claim("email", "johnsmith@example.com")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "password", "name", "role");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync("cookieAuth", claimsPrincipal);

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
          return RedirectToAction("Index", "Home");
        }

        return LocalRedirect(returnUrl);
      }

      return Unauthorized();
    }

    [Authorize]
    public IActionResult Claims()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
