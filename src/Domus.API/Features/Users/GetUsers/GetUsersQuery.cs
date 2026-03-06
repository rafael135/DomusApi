using Domus.Api.Features.Shared;
using Domus.Api.Features.Users.Shared;
using MediatR;

namespace Domus.Api.Features.Users.GetUsers;

public record GetUsersQuery(int pageNumber, int pageSize, string? searchTerm)
    : IRequest<PaginatedResult<UserDto>>;
