using MediatR;

namespace Domus.Api.Features.Transactions.CreateTransaction;

/// <summary>
/// Comando para registrar uma nova transação financeira.
/// </summary>
/// <param name="Description">Descrição da transação.</param>
/// <param name="Value">Valor monetário da transação (deve ser maior que zero).</param>
/// <param name="Type">Tipo da transação (receita ou despesa).</param>
/// <param name="CategoryId">Identificador da categoria associada.</param>
/// <param name="UserId">Identificador do usuário responsável pela transação.</param>
public record CreateTransactionCommand(
    string Description,
    decimal Value,
    TransactionType Type,
    Guid CategoryId,
    Guid UserId
) : IRequest<CreateTransactionResult>;
