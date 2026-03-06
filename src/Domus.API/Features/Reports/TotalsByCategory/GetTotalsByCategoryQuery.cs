using Domus.Core.Domain.Transactions.Enums;
using MediatR;

namespace Domus.Api.Features.Reports.TotalsByCategory;

/// <summary>
/// Query para obter os totais de receitas, despesas e saldo por categoria.
/// </summary>
public record GetTotalsByCategoryQuery() : IRequest<GetTotalsByCategoryResult>;

/// <summary>
/// DTO com os totais financeiros de uma categoria.
/// </summary>
/// <param name="CategoryId">Identificador da categoria.</param>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="Finality">Finalidade da categoria.</param>
/// <param name="TotalIncome">Total de receitas na categoria.</param>
/// <param name="TotalExpense">Total de despesas na categoria.</param>
/// <param name="Balance">Saldo da categoria (receitas menos despesas).</param>
public record CategoryTotalsDto(
    Guid CategoryId,
    string Description,
    TransactionCategoryType Finality,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

/// <summary>
/// Resultado do relatório de totais por categoria, incluindo os totais consolidados.
/// </summary>
/// <param name="Categories">Lista de totais por categoria.</param>
/// <param name="TotalIncome">Receita total consolidada de todas as categorias.</param>
/// <param name="TotalExpense">Despesa total consolidada de todas as categorias.</param>
/// <param name="NetBalance">Saldo líquido consolidado (receita total menos despesa total).</param>
public record GetTotalsByCategoryResult(
    IEnumerable<CategoryTotalsDto> Categories,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal NetBalance
);
