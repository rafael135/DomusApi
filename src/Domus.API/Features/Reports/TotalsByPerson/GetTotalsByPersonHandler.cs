using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domus.Api.Features.Reports.TotalsByPerson;

/// <summary>
/// Handler MediatR responsável por calcular os totais de receitas, despesas e saldo por pessoa.
/// </summary>
public class GetTotalsByPersonHandler
    : IRequestHandler<GetTotalsByPersonQuery, GetTotalsByPersonResult>
{
    private readonly DomusDbContext _dbContext;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    public GetTotalsByPersonHandler(DomusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Processa a query de totais por pessoa, agregando receitas e despesas de cada usuário.
    /// </summary>
    /// <param name="request">Query sem parâmetros.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado com os totais financeiros de cada usuário e os totais consolidados.</returns>
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
