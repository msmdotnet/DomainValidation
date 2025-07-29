namespace DomainValidation.Core;
public class DomainSpecificationsValidator<T>(
    IEnumerable<IDomainSpecification<T>> specifications) : IDomainSpecificationsValidator<T>
{
    public async Task<ValidationResult> ValidateAsync(T entity)
    {
        List<SpecificationError> Errors = [];
        var UnconditionalSpecifications =
            specifications.Where(spec => !spec.EvaluateOnlyIfNoPreviousErrors);

        var ConditionalSpecifications =
            specifications.Where(spec => spec.EvaluateOnlyIfNoPreviousErrors);

        foreach (var Specification in UnconditionalSpecifications)
        {
            if (!await Specification.ValidateAsync(entity))
                Errors.AddRange(Specification.Errors);
        }

        if (Errors.Count == 0)
        {
            var Enumerator = ConditionalSpecifications.GetEnumerator();
            bool IsValid = true;

            while (Enumerator.MoveNext() && IsValid)
            {
                IsValid = await Enumerator.Current.ValidateAsync(entity);
                if (!IsValid)
                    Errors.AddRange(Enumerator.Current.Errors);
            }
        }

        return new ValidationResult(Errors);
    }
}

