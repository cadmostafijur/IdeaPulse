using Microsoft.AspNetCore.Mvc;
using IdeaPulse.Services;
using IdeaPulse.Models;
using IdeaPulse.Models.DTOs;
using IdeaPulse.Data;
using System.Text.Json;

namespace IdeaPulse.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly OpenAIService _openAIService;
    private readonly ApplicationDbContext _context;
    private readonly AuthService _authService;

    public ApiController(
        OpenAIService openAIService,
        ApplicationDbContext context,
        AuthService authService)
    {
        _openAIService = openAIService;
        _context = context;
        _authService = authService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzeIdea([FromBody] AnalyzeIdeaRequest request)
    {
        try
        {
            var userId = _authService.GetCurrentUserId();
            
            // Call AI service
            var analysisResult = await _openAIService.AnalyzeIdeaAsync(request, userId);

            // Save to database
            var ideaAnalysis = new IdeaAnalysis
            {
                StartupName = request.StartupName,
                Description = request.Description,
                Industry = request.Industry,
                Summary = analysisResult.Summary,
                IndustryInsights = JsonSerializer.Serialize(analysisResult.IndustryInsights),
                MarketDemand = JsonSerializer.Serialize(analysisResult.MarketDemand),
                Competitors = JsonSerializer.Serialize(analysisResult.Competitors),
                TargetMarket = JsonSerializer.Serialize(analysisResult.TargetMarket),
                Challenges = JsonSerializer.Serialize(analysisResult.Challenges),
                Recommendations = JsonSerializer.Serialize(analysisResult.Recommendations),
                ValidationScore = analysisResult.ValidationScore,
                UserId = userId ?? 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.IdeaAnalyses.Add(ideaAnalysis);
            await _context.SaveChangesAsync();

            // Set the ID in the result
            analysisResult.Id = ideaAnalysis.Id;
            analysisResult.CreatedAt = ideaAnalysis.CreatedAt;

            return Ok(analysisResult);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("ideas/{id}")]
    public IActionResult GetIdea(int id)
    {
        var idea = _context.IdeaAnalyses.FirstOrDefault(i => i.Id == id);
        if (idea == null)
        {
            return NotFound();
        }

        var analysisResult = new AnalysisResult
        {
            Id = idea.Id,
            StartupName = idea.StartupName,
            Description = idea.Description,
            Industry = idea.Industry,
            Summary = idea.Summary,
            IndustryInsights = JsonSerializer.Deserialize<IndustryInsights>(idea.IndustryInsights),
            MarketDemand = JsonSerializer.Deserialize<MarketDemandInfo>(idea.MarketDemand),
            Competitors = JsonSerializer.Deserialize<List<CompetitorInfo>>(idea.Competitors ?? "[]"),
            TargetMarket = JsonSerializer.Deserialize<TargetMarketInfo>(idea.TargetMarket),
            Challenges = JsonSerializer.Deserialize<List<string>>(idea.Challenges ?? "[]"),
            Recommendations = JsonSerializer.Deserialize<List<string>>(idea.Recommendations ?? "[]"),
            ValidationScore = idea.ValidationScore,
            CreatedAt = idea.CreatedAt
        };

        return Ok(analysisResult);
    }

    [HttpGet("ideas")]
    public IActionResult GetIdeas()
    {
        var userId = _authService.GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var ideas = _context.IdeaAnalyses
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new
            {
                i.Id,
                i.StartupName,
                i.Industry,
                i.ValidationScore,
                i.CreatedAt
            })
            .ToList();

        return Ok(ideas);
    }
}

