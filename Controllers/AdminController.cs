using Microsoft.AspNetCore.Mvc;
using IdeaPulse.Data;
using IdeaPulse.Services;
using Microsoft.EntityFrameworkCore;

namespace IdeaPulse.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AuthService _authService;

    public AdminController(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public IActionResult Dashboard()
    {
        if (_authService.GetCurrentUserRole() != "Admin")
        {
            return Unauthorized();
        }

        var stats = new
        {
            TotalUsers = _context.Users.Count(),
            TotalIdeas = _context.IdeaAnalyses.Count(),
            TotalAIRequests = _context.AIRequestLogs.Count(),
            AvgValidationScore = _context.IdeaAnalyses.Any() 
                ? _context.IdeaAnalyses.Average(i => i.ValidationScore) 
                : 0
        };

        ViewBag.Stats = stats;
        ViewBag.RecentIdeas = _context.IdeaAnalyses
            .Include(i => i.User)
            .OrderByDescending(i => i.CreatedAt)
            .Take(10)
            .ToList();

        ViewBag.RecentUsers = _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(10)
            .ToList();

        return View();
    }

    [HttpGet("users")]
    public IActionResult Users()
    {
        if (_authService.GetCurrentUserRole() != "Admin")
        {
            return Unauthorized();
        }

        var users = _context.Users.Include(u => u.IdeaAnalyses).ToList();
        return View(users);
    }

    [HttpGet("ideas")]
    public IActionResult Ideas()
    {
        if (_authService.GetCurrentUserRole() != "Admin")
        {
            return Unauthorized();
        }

        var ideas = _context.IdeaAnalyses.Include(i => i.User).ToList();
        return View(ideas);
    }

    [HttpGet("logs")]
    public IActionResult Logs()
    {
        if (_authService.GetCurrentUserRole() != "Admin")
        {
            return Unauthorized();
        }

        var logs = _context.AIRequestLogs.OrderByDescending(l => l.Timestamp).Take(100).ToList();
        return View(logs);
    }
}

