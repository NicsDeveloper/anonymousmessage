namespace Nonfy.Api.Contracts.Requests;

public record CreateUserRequest(
    string Email,
    string Password
);
