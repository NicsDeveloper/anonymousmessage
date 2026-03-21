using Microsoft.EntityFrameworkCore;
using Nonfy.Infrastructure;
using Nonfy.Api.Services;
using Nonfy.Api.Contracts.Requests;
using Nonfy.Api.Contracts.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<NonfyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Register endpoints
app.MapPost("/users", CreateUser)
    .WithName("CreateUser")
    .WithOpenApi()
    .Produces<CreateUserResponse>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict);

app.Run();

// Endpoint handler
async Task<IResult> CreateUser(CreateUserRequest request, IUserService userService)
{
    try
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { error = "Email and password are required." });
        }

        var user = await userService.RegisterUserAsync(request.Email, request.Password);

        var response = new CreateUserResponse(user.Id, user.Email, user.Slug);
        return Results.Created($"/users/{user.Id}", response);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException?.Message.Contains("Email") ?? false)
    {
        return Results.Conflict(new { error = "Email already exists." });
    }
    catch (Exception)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}
