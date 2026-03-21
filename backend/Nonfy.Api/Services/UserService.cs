using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Nonfy.Domain.Entities;
using Nonfy.Infrastructure;

namespace Nonfy.Api.Services;

public interface IUserService
{
    Task<User> RegisterUserAsync(string email, string password);
}

public class UserService : IUserService
{
    private readonly NonfyDbContext _context;

    // Password complexity regex: at least 1 uppercase, 1 lowercase, 1 digit, 1 symbol
    private static readonly Regex PasswordComplexityRegex = new(
        @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':\"",.<>?/\\|`~])",
        RegexOptions.Compiled
    );

    public UserService(NonfyDbContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterUserAsync(string email, string password)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(email))
            throw new ArgumentException("Email format is invalid.", nameof(email));

        // Validate password strength
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        if (password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));

        if (!PasswordComplexityRegex.IsMatch(password))
            throw new ArgumentException(
                "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*()_+-=[]{}';:\",.<>?/\\|`~).",
                nameof(password)
            );

        // Generate slug from email (e.g., "john@example.com" -> "john")
        var slug = email.Split('@')[0].ToLowerInvariant();

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // Create and persist user
        var user = new User(email, email, passwordHash, slug);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
