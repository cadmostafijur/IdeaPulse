namespace IdeaPulse.Models.DTOs;

public class AnalyzeIdeaRequest
{
    public string StartupName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Industry { get; set; }
}

