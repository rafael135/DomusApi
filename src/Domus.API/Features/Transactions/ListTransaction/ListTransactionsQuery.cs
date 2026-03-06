using Domus.Api.Features.Shared;
using Domus.Api.Features.Transactions.Shared;
using MediatR;

namespace Domus.Api.Features.Transactions.ListTransaction;

public record ListTransactionsQuery(int PageNumber, int PageSize, Guid? UserId, Guid? CategoryId)
    : IRequest<PaginatedResult<TransactionDto>>;
