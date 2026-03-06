namespace Domus.Core.Domain.Shared.Exceptions;

/// <summary>
/// Exceção lançada quando uma entidade solicitada não é encontrada.
/// </summary>
public class NotFoundException : DomainException
{
    /// <summary>
    /// Cria uma nova <see cref="NotFoundException"/> para a entidade e chave especificadas.
    /// </summary>
    /// <param name="name">Nome da entidade não encontrada.</param>
    /// <param name="key">Chave utilizada na busca.</param>
    public NotFoundException(string name, object key)
        : base($"Entidade \"{name}\" ({key}) não foi encontrada.") { }
}
