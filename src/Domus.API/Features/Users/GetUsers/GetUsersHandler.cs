using Domus.Api.Features.Shared;
using Domus.Api.Features.Users.Shared;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginatedResult<UserDto>>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<GetUsersHandler> _logger;

    public GetUsersHandler(DomusDbContext dbContext, ILogger<GetUsersHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PaginatedResult<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(request.searchTerm))
        {
            query = query.Where(u => u.Name.Contains(request.searchTerm));
        }

        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)request.pageSize);


        var result = query
            .Skip((request.pageNumber - 1) * request.pageSize)
            .Take(request.pageSize)
            .Select(u => new UserDto(u.Id, u.Name, u.Age))
            .ToList();


        return new PaginatedResult<UserDto>(
            result,
            totalPages,
            totalItems,
            request.pageNumber,
            request.pageSize
        );
    }
}
