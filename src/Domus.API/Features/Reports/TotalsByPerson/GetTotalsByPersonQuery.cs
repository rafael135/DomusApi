using MediatR;

namespace Domus.Api.Features.Reports.TotalsByPerson;

public record GetTotalsByPersonQuery() : IRequest<GetTotalsByPersonResult>;

public record PersonTotalsDto(
    Guid PersonId,
    string PersonName,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

public record GetTotalsByPersonResult(
    IEnumerable<PersonTotalsDto> Persons,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal NetBalance
);
