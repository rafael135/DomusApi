using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.DeleteUser;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/api/users/{userId:guid}",
                async (IMediator mediator, Guid userId) =>
                {
                    var command = new DeleteUserCommand(userId);
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                }
            )
            .Produces<DeleteUserResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
