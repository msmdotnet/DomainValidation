namespace DomainValidation.Interfaces;
public interface IDomainSpecification<T>
{
    bool EvaluateOnlyIfNoPreviousErrors { get; }
    bool StopOnFirstEntitySpecificationError { get; }
    IEnumerable<SpecificationError> Errors { get; }
    Task<bool> ValidateAsync(T entity);
}

