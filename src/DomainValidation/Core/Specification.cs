namespace DomainValidation.Core;
public class Specification<T>(
    Func<T, IEnumerable<SpecificationError>> validationRule) : ISpecification<T>
{
    public IEnumerable<SpecificationError> Errors { get; private set; }
    public bool IsSatisfiedBy(T entity)
    {
        Errors = validationRule(entity);

        return Errors?.Any() != true;
    }
}

