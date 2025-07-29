namespace DomainValidation.Interfaces;
public interface IDomainSpecificationsValidator<T>
{
    Task<ValidationResult> ValidateAsync(T entity);
}
