using System.Text;
using System.Text.Json;
using IdeaPulse.Models;
using IdeaPulse.Models.DTOs;
using IdeaPulse.Data;

namespace IdeaPulse.Services;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OpenAIService> _logger;

    public OpenAIService(
        HttpClient httpClient, 
        IConfiguration configuration,
        ApplicationDbContext context,
        ILogger<OpenAIService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key not configured");
        _context = context;
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<AnalysisResult> AnalyzeIdeaAsync(AnalyzeIdeaRequest request, int? userId = null)
    {
        try
        {
            var prompt = BuildPrompt(request);
            
            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = "You are an expert startup analyst. Provide detailed market validation analysis in JSON format." },
                    new { role = "user", content = prompt }
                },
                response_format = new { type = "json_object" },
                temperature = 0.7
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("chat/completions", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                var aiResponse = jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                
                await LogRequestAsync("chat/completions", jsonContent, responseContent, userId, true);
                
                return ParseAIResponse(aiResponse, request);
            }
            else
            {
                await LogRequestAsync("chat/completions", jsonContent, responseContent, userId, false, responseContent);
                throw new Exception($"OpenAI API error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing idea with OpenAI");
            await LogRequestAsync("chat/completions", request.Description, "", userId, false, ex.Message);
            throw;
        }
    }

    private string BuildPrompt(AnalyzeIdeaRequest request)
    {
        return $@"Analyze this startup idea and provide a comprehensive market validation report in JSON format.

Startup Name: {request.StartupName}
Description: {request.Description}
Industry: {request.Industry ?? "General"}

Provide your analysis as a JSON object with the following structure:
{{
  ""summary"": ""2-3 sentence overview of the startup idea"",
  ""industryInsights"": {{
    ""sector"": ""identified industry sector"",
    ""overview"": ""detailed industry analysis (200-300 words)""
  }},
  ""targetMarket"": {{
    ""customerGroup"": ""primary customer segment"",
    ""demographics"": ""target customer demographics and characteristics""
  }},
  ""marketDemand"": {{
    ""demandLevel"": ""high/medium/low"",
    ""analysis"": ""detailed demand analysis (200-300 words)"",
    ""growthPotential"": 0-100
  }},
  ""competitors"": [
    {{
      ""name"": ""competitor name"",
      ""description"": ""what they do"",
      ""competitiveEdge"": ""how your idea differs""
    }}
  ],
  ""challenges"": [""challenge 1"", ""challenge 2"", ""challenge 3""],
  ""recommendations"": [""recommendation 1"", ""recommendation 2"", ""recommendation 3""],
  ""validationScore"": 0-100
}}

Base the validation score on: market demand (30%), competitive landscape (20%), feasibility (20%), innovation level (15%), growth potential (15%). Return ONLY valid JSON.";
    }

    private AnalysisResult ParseAIResponse(string? aiResponse, AnalyzeIdeaRequest request)
    {
        if (string.IsNullOrEmpty(aiResponse))
        {
            throw new Exception("Empty AI response");
        }

        var json = JsonSerializer.Deserialize<JsonElement>(aiResponse);
        
        var result = new AnalysisResult
        {
            StartupName = request.StartupName,
            Description = request.Description,
            Industry = request.Industry,
            Summary = json.GetProperty("summary").GetString() ?? "",
            ValidationScore = json.TryGetProperty("validationScore", out var score) ? score.GetInt32() : 50
        };

        // Parse industry insights
        if (json.TryGetProperty("industryInsights", out var insights))
        {
            result.IndustryInsights = new IndustryInsights
            {
                Sector = insights.GetProperty("sector").GetString() ?? "",
                Overview = insights.GetProperty("overview").GetString() ?? ""
            };
        }

        // Parse market demand
        if (json.TryGetProperty("marketDemand", out var demand))
        {
            result.MarketDemand = new MarketDemandInfo
            {
                DemandLevel = demand.GetProperty("demandLevel").GetString() ?? "",
                Analysis = demand.GetProperty("analysis").GetString() ?? "",
                GrowthPotential = demand.TryGetProperty("growthPotential", out var gp) ? gp.GetInt32() : 50
            };
        }

        // Parse competitors
        if (json.TryGetProperty("competitors", out var competitors) && competitors.ValueKind == JsonValueKind.Array)
        {
            result.Competitors = new List<CompetitorInfo>();
            foreach (var comp in competitors.EnumerateArray())
            {
                result.Competitors.Add(new CompetitorInfo
                {
                    Name = comp.GetProperty("name").GetString() ?? "",
                    Description = comp.GetProperty("description").GetString() ?? "",
                    CompetitiveEdge = comp.GetProperty("competitiveEdge").GetString() ?? ""
                });
            }
        }

        // Parse target market
        if (json.TryGetProperty("targetMarket", out var target))
        {
            result.TargetMarket = new TargetMarketInfo
            {
                CustomerGroup = target.GetProperty("customerGroup").GetString() ?? "",
                Demographics = target.GetProperty("demographics").GetString() ?? ""
            };
        }

        // Parse challenges
        if (json.TryGetProperty("challenges", out var challenges) && challenges.ValueKind == JsonValueKind.Array)
        {
            result.Challenges = challenges.EnumerateArray().Select(c => c.GetString() ?? "").ToList();
        }

        // Parse recommendations
        if (json.TryGetProperty("recommendations", out var recommendations) && recommendations.ValueKind == JsonValueKind.Array)
        {
            result.Recommendations = recommendations.EnumerateArray().Select(r => r.GetString() ?? "").ToList();
        }

        return result;
    }

    private async Task LogRequestAsync(string endpoint, string requestData, string? responseData, int? userId, bool success, string? errorMessage = null)
    {
        try
        {
            var log = new AIRequestLog
            {
                Endpoint = endpoint,
                RequestData = requestData.Length > 5000 ? requestData[..5000] : requestData,
                ResponseData = responseData?.Length > 5000 ? responseData[..5000] : responseData,
                UserId = userId,
                Success = success,
                ErrorMessage = errorMessage
            };

            _context.AIRequestLogs.Add(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging AI request");
        }
    }
}

