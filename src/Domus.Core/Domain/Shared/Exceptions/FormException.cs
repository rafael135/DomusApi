namespace Domus.Core.Domain.Shared.Exceptions;

/// <summary>
/// Exceção que representa um conjunto de erros de validação de campos de entrada.
/// </summary>
public class FormException : DomainException
{
    /// <summary>Dicionário mapeando o nome do campo ao respectivo erro de validação.</summary>
    public Dictionary<string, string> Errors { get; }

    /// <summary>
    /// Cria uma nova <see cref="FormException"/> com os erros de validação fornecidos.
    /// </summary>
    /// <param name="errors">Dicionário de erros por campo.</param>
    public FormException(Dictionary<string, string> errors)
        : base("Um ou mais erros de validação ocorreram.")
    {
        Errors = errors;
    }
}
