using Domus.Api.Features.Shared;
using Domus.Api.Features.Transactions.Shared;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Transactions.ListTransaction;

public class ListTransactionsHandler
    : IRequestHandler<ListTransactionsQuery, PaginatedResult<TransactionDto>>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<ListTransactionsHandler> _logger;

    public ListTransactionsHandler(
        DomusDbContext dbContext,
        ILogger<ListTransactionsHandler> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PaginatedResult<TransactionDto>> Handle(
        ListTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = _dbContext.Transactions.AsQueryable();

        if (request.UserId.HasValue)
            query = query.Where(t => t.UserId == request.UserId.Value);

        if (request.CategoryId.HasValue)
            query = query.Where(t => t.TransactionCategoryId == request.CategoryId.Value);

        int totalItems = await query.CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TransactionDto(
                t.Id,
                t.Description,
                t.Value,
                t.Type,
                t.TransactionCategoryId,
                t.TransactionCategory.Description,
                t.UserId,
                t.User.Name
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<TransactionDto>(
            items,
            totalPages,
            totalItems,
            request.PageNumber,
            request.PageSize
        );
    }
}
