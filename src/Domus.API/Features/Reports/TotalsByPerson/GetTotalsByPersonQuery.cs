using MediatR;

namespace Domus.Api.Features.Reports.TotalsByPerson;

/// <summary>
/// Query para obter os totais de receitas, despesas e saldo por pessoa.
/// </summary>
public record GetTotalsByPersonQuery() : IRequest<GetTotalsByPersonResult>;

/// <summary>
/// DTO com os totais financeiros de uma pessoa.
/// </summary>
/// <param name="PersonId">Identificador do usuário.</param>
/// <param name="PersonName">Nome do usuário.</param>
/// <param name="TotalIncome">Total de receitas registradas.</param>
/// <param name="TotalExpense">Total de despesas registradas.</param>
/// <param name="Balance">Saldo (receitas menos despesas).</param>
public record PersonTotalsDto(
    Guid PersonId,
    string PersonName,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

/// <summary>
/// Resultado do relatório de totais por pessoa, incluindo os totais consolidados.
/// </summary>
/// <param name="Persons">Lista de totais por pessoa.</param>
/// <param name="TotalIncome">Receita total consolidada de todas as pessoas.</param>
/// <param name="TotalExpense">Despesa total consolidada de todas as pessoas.</param>
/// <param name="NetBalance">Saldo líquido consolidado (receita total menos despesa total).</param>
public record GetTotalsByPersonResult(
    IEnumerable<PersonTotalsDto> Persons,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal NetBalance
);
