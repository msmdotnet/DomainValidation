namespace DomainValidation.Interfaces;
public interface ISpecification<T>
{
    IEnumerable<SpecificationError> Errors { get; }
    bool IsSatisfiedBy(T entity);
}

