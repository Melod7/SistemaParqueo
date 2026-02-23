using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaParqueo.MVC.Models;
using SistemaParqueo.MVC.Services;

public class AccountController : Controller
{
    private readonly ApiService _apiService;

    public AccountController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string usuario)
    {
        if (string.IsNullOrEmpty(usuario))
        {
            ViewBag.Error = "Ingrese un usuario";
            return View();
        }

        HttpContext.Session.SetString("Usuario", usuario);

        return RedirectToAction("Panel", "Parqueo");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}