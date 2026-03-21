namespace Nonfy.Api.Contracts.Responses;

public record CreateUserResponse(
    Guid Id,
    string Email,
    string Slug
);
