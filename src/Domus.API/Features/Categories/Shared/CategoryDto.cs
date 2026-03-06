using Domus.Core.Domain.Transactions.Enums;

namespace Domus.Api.Features.Categories.Shared;

/// <summary>
/// DTO de categoria de transação utilizado nas respostas da API.
/// </summary>
/// <param name="Id">Identificador único da categoria.</param>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="Finality">Finalidade da categoria (despesa, receita ou ambos).</param>
public record CategoryDto(Guid Id, string Description, TransactionCategoryType Finality);
