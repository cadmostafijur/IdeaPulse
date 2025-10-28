using Microsoft.AspNetCore.Mvc;
using IdeaPulse.Services;
using IdeaPulse.Models.DTOs;
using IdeaPulse.Data;
using System.Text.Json;

namespace IdeaPulse.Controllers;

public class PDFController : Controller
{
    private readonly PDFService _pdfService;
    private readonly ApplicationDbContext _context;

    public PDFController(PDFService pdfService, ApplicationDbContext context)
    {
        _pdfService = pdfService;
        _context = context;
    }

    [HttpGet("generate/{id}")]
    public IActionResult GeneratePDF(int id)
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
            IndustryInsights = JsonSerializer.Deserialize<IndustryInsights>(idea.IndustryInsights ?? "{}"),
            MarketDemand = JsonSerializer.Deserialize<MarketDemandInfo>(idea.MarketDemand ?? "{}"),
            Competitors = JsonSerializer.Deserialize<List<CompetitorInfo>>(idea.Competitors ?? "[]"),
            TargetMarket = JsonSerializer.Deserialize<TargetMarketInfo>(idea.TargetMarket ?? "{}"),
            Challenges = JsonSerializer.Deserialize<List<string>>(idea.Challenges ?? "[]"),
            Recommendations = JsonSerializer.Deserialize<List<string>>(idea.Recommendations ?? "[]"),
            ValidationScore = idea.ValidationScore,
            CreatedAt = idea.CreatedAt
        };

        var pdfBytes = _pdfService.GenerateReport(analysisResult);

        return File(pdfBytes, "application/pdf", $"{idea.StartupName}_Validation_Report.pdf");
    }
}

