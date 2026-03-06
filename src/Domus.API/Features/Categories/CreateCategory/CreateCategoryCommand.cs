using Domus.Core.Domain.Transactions.Enums;
using MediatR;

namespace Domus.Api.Features.Categories.CreateCategory;

public record CreateCategoryCommand(string Description, TransactionCategoryType Finality)
    : IRequest<CreateCategoryResult>;
