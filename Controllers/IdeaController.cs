using Microsoft.AspNetCore.Mvc;
using IdeaPulse.Services;

namespace IdeaPulse.Controllers;

public class IdeaController : Controller
{
    private readonly AuthService _authService;

    public IdeaController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Analyzer()
    {
        ViewBag.UserId = _authService.GetCurrentUserId();
        return View();
    }

    public IActionResult Result(int id)
    {
        ViewBag.IdeaId = id;
        return View();
    }
}

