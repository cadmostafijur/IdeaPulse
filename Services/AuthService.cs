using BCrypt.Net;
using IdeaPulse.Data;
using IdeaPulse.Models;
using IdeaPulse.Models.DTOs;

namespace IdeaPulse.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public Task<int?> LoginAsync(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.IsActive);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return Task.FromResult<int?>(null);
        }

        // Set session
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            session.SetInt32("UserId", user.Id);
            session.SetString("UserName", user.Name);
            session.SetString("UserRole", user.Role);
        }

        return Task.FromResult<int?>(user.Id);
    }

    public async Task<int> RegisterAsync(UserRegistrationDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
        {
            throw new Exception("Email already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = dto.Role,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Auto login
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            session.SetInt32("UserId", user.Id);
            session.SetString("UserName", user.Name);
            session.SetString("UserRole", user.Role);
        }

        return user.Id;
    }

    public void Logout()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Clear();
    }

    public int? GetCurrentUserId()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        return session?.GetInt32("UserId");
    }

    public string? GetCurrentUserRole()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        return session?.GetString("UserRole");
    }
}

// DTO for registration
public class UserRegistrationDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Entrepreneur";
}

