using Domus.Core.Domain.Transactions.Enums;

namespace Domus.Api.Features.Categories.Shared;

public record CategoryDto(Guid Id, string Description, TransactionCategoryType Finality);
