namespace DomainValidation.Guards;
public static class DomainValidationGuard
{
    public static async Task AgainstInvalidSpecification<T>(
        IDomainSpecificationsValidator<T> validator, T entity,
        string message = default)
    {
        var ValidationResult = await validator.ValidateAsync(entity);

        if (!ValidationResult.IsValid)
            throw new DomainValidationException(
                ValidationResult.Errors, message);
    }
}

