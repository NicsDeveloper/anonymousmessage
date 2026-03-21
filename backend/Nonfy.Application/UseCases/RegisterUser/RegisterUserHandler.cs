using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Nonfy.Application.Interfaces;
using Nonfy.Domain.Entities;
using Nonfy.Domain.Repositories;

namespace Nonfy.Application.UseCases.RegisterUser;

public class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    private static readonly Regex PasswordComplexityRegex = new(
        @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':'"".,<>?/\\|`~])",
        RegexOptions.Compiled
    );

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResult> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(command.Email));

        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(command.Email))
            throw new ArgumentException("Email format is invalid.", nameof(command.Email));

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(command.Password));

        if (command.Password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long.", nameof(command.Password));

        if (!PasswordComplexityRegex.IsMatch(command.Password))
            throw new ArgumentException(
                "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*()_+-=[]{}';:\",.<>?/\\|`~).",
                nameof(command.Password)
            );

        var slug = command.Email.Split('@')[0].ToLowerInvariant();
        var passwordHash = _passwordHasher.Hash(command.Password);

        // TODO: BusinessName will be collected from the user in a future endpoint update
        var user = new User(command.Email, command.Email, passwordHash, slug);

        await _userRepository.AddAsync(user, cancellationToken);

        return new RegisterUserResult(user.Id, user.Email, user.Slug);
    }
}
