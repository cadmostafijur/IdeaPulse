using Microsoft.AspNetCore.Mvc;
using IdeaPulse.Services;
using IdeaPulse.Models;
using IdeaPulse.Models.DTOs;
using IdeaPulse.Data;

namespace IdeaPulse.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AuthService _authService;

    public HomeController(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public IActionResult Index()
    {
        ViewBag.IsLoggedIn = _authService.GetCurrentUserId() != null;
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Terms()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitContact(string name, string email, string message)
    {
        // In production, send email using email service
        TempData["Message"] = "Thank you for your message! We'll get back to you soon.";
        return RedirectToAction("Contact");
    }
}

