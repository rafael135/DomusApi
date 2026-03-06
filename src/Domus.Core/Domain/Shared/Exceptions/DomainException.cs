namespace Domus.Core.Domain.Shared.Exceptions;

/// <summary>
/// Exceção base para erros originados na camada de domínio.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Cria uma nova <see cref="DomainException"/> com a mensagem especificada.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    public DomainException(string message)
        : base(message) { }
}
