using Domus.Core.Domain.Shared;
using Domus.Core.Domain.Shared.Exceptions;

namespace Domus.Core;

/// <summary>
/// Classe base para todas as entidades do domínio.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Construtor protegido para uso pelo EF Core.
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// Inicializa a entidade com um identificador específico.
    /// </summary>
    /// <param name="id">O identificador único da entidade.</param>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Determina se o objeto especificado é igual à entidade atual, comparando por identidade (<see cref="Id"/>).
    /// </summary>
    /// <param name="obj">O objeto a ser comparado com a entidade atual.</param>
    /// <returns><c>true</c> se os objetos são do mesmo tipo e possuem o mesmo <see cref="Id"/>; caso contrário, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (Id == Guid.Empty || other.Id == Guid.Empty)
        {
            return false;
        }

        return Id == other.Id;
    }

    /// <summary>
    /// Retorna o código de hash da entidade baseado no tipo e no <see cref="Id"/>.
    /// </summary>
    /// <returns>Código de hash da entidade.</returns>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    /// <summary>
    /// Verifica a regra de negócio especificada e lança <see cref="BusinessRuleException"/> caso a regra seja violada.
    /// </summary>
    /// <param name="rule">A regra de negócio a ser avaliada.</param>
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleException(rule);
        }
    }
}
