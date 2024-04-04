using Microsoft.AspNetCore.Mvc;

namespace Newsletter.MVC.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        var claims = HttpContext.User.Claims.ToList();
        return View();
    }
}
