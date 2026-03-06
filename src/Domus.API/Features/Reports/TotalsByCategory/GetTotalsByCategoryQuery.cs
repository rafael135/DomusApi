using Domus.Core.Domain.Transactions.Enums;
using MediatR;

namespace Domus.Api.Features.Reports.TotalsByCategory;

public record GetTotalsByCategoryQuery() : IRequest<GetTotalsByCategoryResult>;

public record CategoryTotalsDto(
    Guid CategoryId,
    string Description,
    TransactionCategoryType Finality,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

public record GetTotalsByCategoryResult(
    IEnumerable<CategoryTotalsDto> Categories,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal NetBalance
);
