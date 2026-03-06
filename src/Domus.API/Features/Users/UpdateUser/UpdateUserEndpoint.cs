using Domus.Api.Extensions;
using Domus.Api.Features.Users.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.UpdateUser;

public class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/users/{userId:guid}",
                async (IMediator mediator, Guid userId, [FromBody] UpdateUserBody body) =>
                {
                    var command = new UpdateUserCommand(userId, body.Name, body.Age);
                    var result = await mediator.Send(command);
                    return Results.Ok(result.User);
                }
            )
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithSummary("Update an existing user.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}

public record UpdateUserBody(string Name, int Age);
