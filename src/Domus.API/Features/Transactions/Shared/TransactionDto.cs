namespace Domus.Api.Features.Transactions.Shared;

/// <summary>
/// DTO de transação financeira utilizado nas respostas da API.
/// </summary>
/// <param name="Id">Identificador único da transação.</param>
/// <param name="Description">Descrição da transação.</param>
/// <param name="Value">Valor monetário da transação.</param>
/// <param name="Type">Tipo da transação (receita ou despesa).</param>
/// <param name="CategoryId">Identificador da categoria associada.</param>
/// <param name="CategoryDescription">Descrição da categoria associada.</param>
/// <param name="UserId">Identificador do usuário proprietário da transação.</param>
/// <param name="UserName">Nome do usuário proprietário da transação.</param>
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
