using Microsoft.AspNetCore.Mvc;
using IdeaPulse.Services;
using IdeaPulse.Models;

namespace IdeaPulse.Controllers;

public class AccountController : Controller
{
    private readonly AuthService _authService;

    public AccountController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Login()
    {
        if (_authService.GetCurrentUserId() != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            var userId = await _authService.LoginAsync(email, password);
            if (userId != null)
            {
                return RedirectToAction("Dashboard", "Account");
            }
            
            ViewBag.Error = "Invalid email or password";
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }

    public IActionResult Signup()
    {
        if (_authService.GetCurrentUserId() != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup(UserRegistrationDto dto)
    {
        try
        {
            await _authService.RegisterAsync(dto);
            return RedirectToAction("Dashboard", "Account");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        if (_authService.GetCurrentUserId() == null)
        {
            return RedirectToAction("Login");
        }
        
        ViewBag.UserId = _authService.GetCurrentUserId();
        ViewBag.UserRole = _authService.GetCurrentUserRole();
        
        return View();
    }

    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Index", "Home");
    }
}

