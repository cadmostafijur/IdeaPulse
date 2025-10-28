namespace IdeaPulse.Models;

public class AIRequestLog
{
    public int Id { get; set; }
    
    public string Endpoint { get; set; } = string.Empty;
    public string RequestData { get; set; } = string.Empty;
    public string? ResponseData { get; set; }
    public int? UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

