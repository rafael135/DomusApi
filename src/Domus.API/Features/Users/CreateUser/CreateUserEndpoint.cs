using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Endpoint for creating a new user.
/// </summary>
/// <remarks>
/// Example request: { "name": "Alice", "age": 25 }
/// </remarks>
public class CreateUserEndpoint : IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the application route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/users",
                async (IMediator mediator, [FromBody] CreateUserCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(result.User);
                }
            )
            .WithName("CreateUser")
            .WithTags("Users")
            .WithSummary("Create a new user.")
            .WithDescription("Creates a new user.")
            .Produces<CreateUserResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}
