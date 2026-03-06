namespace Domus.Core.Domain.Shared;

/// <summary>
/// Contrato para regras de negócio do domínio.
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// Verifica se a regra de negócio foi violada.
    /// </summary>
    /// <returns><c>true</c> se a regra está violada; caso contrário, <c>false</c>.</returns>
    bool IsBroken();

    /// <summary>
    /// Mensagem descritiva da violação da regra.
    /// </summary>
    string Message { get; }
}
