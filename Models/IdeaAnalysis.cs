using System.ComponentModel.DataAnnotations;

namespace IdeaPulse.Models;

public class IdeaAnalysis
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string StartupName { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? Industry { get; set; }
    
    // JSON fields for AI-generated data
    public string IndustryInsights { get; set; } = "{}";
    public string MarketDemand { get; set; } = "{}";
    public string Competitors { get; set; } = "[]";
    public string TargetMarket { get; set; } = "{}";
    public string Challenges { get; set; } = "[]";
    public string Recommendations { get; set; } = "[]";
    public string Summary { get; set; } = string.Empty;
    
    public int ValidationScore { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public int UserId { get; set; }
    public User? User { get; set; }
}

