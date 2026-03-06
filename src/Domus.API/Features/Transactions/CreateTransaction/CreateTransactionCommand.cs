using MediatR;

namespace Domus.Api.Features.Transactions.CreateTransaction;

public record CreateTransactionCommand(
    string Description,
    decimal Value,
    TransactionType Type,
    Guid CategoryId,
    Guid UserId
) : IRequest<CreateTransactionResult>;
