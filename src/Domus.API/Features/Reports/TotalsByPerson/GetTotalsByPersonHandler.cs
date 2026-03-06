using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domus.Api.Features.Reports.TotalsByPerson;

public class GetTotalsByPersonHandler
    : IRequestHandler<GetTotalsByPersonQuery, GetTotalsByPersonResult>
{
    private readonly DomusDbContext _dbContext;

    public GetTotalsByPersonHandler(DomusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetTotalsByPersonResult> Handle(
        GetTotalsByPersonQuery request,
        CancellationToken cancellationToken
    )
    {
        var persons = await _dbContext
            .Users.Select(u => new PersonTotalsDto(
                u.Id,
                u.Name,
                u.Transactions.Where(t => t.Type == TransactionType.Income)
                    .Sum(t => (decimal?)t.Value)
                ?? 0m,
                u.Transactions.Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => (decimal?)t.Value)
                ?? 0m,
                (
                    u.Transactions.Where(t => t.Type == TransactionType.Income)
                        .Sum(t => (decimal?)t.Value)
                    ?? 0m
                )
                    - (
                        u.Transactions.Where(t => t.Type == TransactionType.Expense)
                            .Sum(t => (decimal?)t.Value)
                        ?? 0m
                    )
            ))
            .ToListAsync(cancellationToken);

        decimal totalIncome = persons.Sum(p => p.TotalIncome);
        decimal totalExpense = persons.Sum(p => p.TotalExpense);

        return new GetTotalsByPersonResult(
            persons,
            totalIncome,
            totalExpense,
            totalIncome - totalExpense
        );
    }
}
