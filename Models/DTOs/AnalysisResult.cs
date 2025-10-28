namespace IdeaPulse.Models.DTOs;

public class AnalysisResult
{
    public int Id { get; set; }
    public string StartupName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string Summary { get; set; } = string.Empty;
    public IndustryInsights? IndustryInsights { get; set; }
    public MarketDemandInfo? MarketDemand { get; set; }
    public List<CompetitorInfo>? Competitors { get; set; }
    public TargetMarketInfo? TargetMarket { get; set; }
    public List<string>? Challenges { get; set; }
    public List<string>? Recommendations { get; set; }
    public int ValidationScore { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class IndustryInsights
{
    public string Sector { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
}

public class MarketDemandInfo
{
    public string DemandLevel { get; set; } = string.Empty;
    public string Analysis { get; set; } = string.Empty;
    public int GrowthPotential { get; set; }
}

public class CompetitorInfo
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CompetitiveEdge { get; set; } = string.Empty;
}

public class TargetMarketInfo
{
    public string CustomerGroup { get; set; } = string.Empty;
    public string Demographics { get; set; } = string.Empty;
}
