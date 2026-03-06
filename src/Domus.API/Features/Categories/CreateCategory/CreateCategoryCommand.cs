using Domus.Core.Domain.Transactions.Enums;
using MediatR;

namespace Domus.Api.Features.Categories.CreateCategory;

/// <summary>
/// Comando para criar uma nova categoria de transação.
/// </summary>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="Finality">Finalidade da categoria (despesa, receita ou ambos).</param>
public record CreateCategoryCommand(string Description, TransactionCategoryType Finality)
    : IRequest<CreateCategoryResult>;
