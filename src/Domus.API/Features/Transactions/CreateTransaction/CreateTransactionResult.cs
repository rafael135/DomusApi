using Domus.Api.Features.Transactions.Shared;

namespace Domus.Api.Features.Transactions.CreateTransaction;

/// <summary>
/// Resultado da operação de criação de transação.
/// </summary>
/// <param name="Transaction">DTO com os dados da transação criada.</param>
public record CreateTransactionResult(TransactionDto Transaction);
