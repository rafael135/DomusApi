using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domus.Api.Features.Reports.TotalsByCategory;

public class GetTotalsByCategoryHandler
    : IRequestHandler<GetTotalsByCategoryQuery, GetTotalsByCategoryResult>
{
    private readonly DomusDbContext _dbContext;

    public GetTotalsByCategoryHandler(DomusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetTotalsByCategoryResult> Handle(
        GetTotalsByCategoryQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = await _dbContext
            .TransactionCategories.Select(c => new CategoryTotalsDto(
                c.Id,
                c.Description,
                c.Finality,
                c.Transactions.Where(t => t.Type == TransactionType.Income)
                    .Sum(t => (decimal?)t.Value)
                ?? 0m,
                c.Transactions.Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => (decimal?)t.Value)
                ?? 0m,
                (
                    c.Transactions.Where(t => t.Type == TransactionType.Income)
                        .Sum(t => (decimal?)t.Value)
                    ?? 0m
                )
                    - (
                        c.Transactions.Where(t => t.Type == TransactionType.Expense)
                            .Sum(t => (decimal?)t.Value)
                        ?? 0m
                    )
            ))
            .ToListAsync(cancellationToken);

        decimal totalIncome = categories.Sum(c => c.TotalIncome);
        decimal totalExpense = categories.Sum(c => c.TotalExpense);

        return new GetTotalsByCategoryResult(
            categories,
            totalIncome,
            totalExpense,
            totalIncome - totalExpense
        );
    }
}
