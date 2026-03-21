namespace Nonfy.Application.UseCases.RegisterUser;

public record RegisterUserCommand(string Email, string Password);

public record RegisterUserResult(Guid Id, string Email, string Slug);
