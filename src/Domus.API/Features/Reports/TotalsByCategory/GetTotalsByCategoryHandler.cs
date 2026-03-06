using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domus.Api.Features.Reports.TotalsByCategory;

/// <summary>
/// Handler MediatR responsável por calcular os totais de receitas, despesas e saldo por categoria.
/// </summary>
public class GetTotalsByCategoryHandler
    : IRequestHandler<GetTotalsByCategoryQuery, GetTotalsByCategoryResult>
{
    private readonly DomusDbContext _dbContext;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    public GetTotalsByCategoryHandler(DomusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Processa a query de totais por categoria, agregando receitas e despesas em cada categoria.
    /// </summary>
    /// <param name="request">Query sem parâmetros.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado com os totais financeiros de cada categoria e os totais consolidados.</returns>
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
