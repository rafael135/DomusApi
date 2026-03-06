namespace Domus.Core.Domain.Shared.Exceptions;

/// <summary>
/// Exceção lançada quando uma regra de negócio do domínio é violada.
/// </summary>
public class BusinessRuleException : DomainException
{
    /// <summary>A regra de negócio que foi violada.</summary>
    public IBusinessRule BrokenRule { get; }

    /// <summary>
    /// Cria uma nova <see cref="BusinessRuleException"/> para a regra violada especificada.
    /// </summary>
    /// <param name="brokenRule">A regra que foi violada.</param>
    public BusinessRuleException(IBusinessRule brokenRule)
        : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{BrokenRule.GetType().Name}: {BrokenRule.Message}";
    }
}
