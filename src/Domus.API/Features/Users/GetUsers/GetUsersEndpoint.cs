using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.GetUsers;

public class GetUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/users",
                async (
                    IMediator mediator,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 10,
                    [FromQuery] string? searchTerm = null
                ) =>
                {
                    GetUsersQuery query = new GetUsersQuery(
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                        searchTerm: searchTerm
                    );
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                }
            )
            .WithName("GetUsers")
            .WithTags("Users");
    }
}
