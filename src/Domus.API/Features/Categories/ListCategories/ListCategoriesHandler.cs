using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Shared;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Categories.ListCategories;

public class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, PaginatedResult<CategoryDto>>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<ListCategoriesHandler> _logger;

    public ListCategoriesHandler(DomusDbContext dbContext, ILogger<ListCategoriesHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PaginatedResult<CategoryDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.TransactionCategories.AsQueryable();

        if (request.Finality.HasValue)
            query = query.Where(c => c.Finality == request.Finality.Value);

        int totalItems = await query.CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategoryDto(c.Id, c.Description, c.Finality))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CategoryDto>(items, totalPages, totalItems, request.PageNumber, request.PageSize);
    }
}
