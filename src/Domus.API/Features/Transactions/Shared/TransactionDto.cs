namespace Domus.Api.Features.Transactions.Shared;

public record TransactionDto(
    Guid Id,
    string Description,
    decimal Value,
    TransactionType Type,
    Guid CategoryId,
    string CategoryDescription,
    Guid UserId,
    string UserName
);
