namespace DomainValidation.ValueObjects;
public class ValidationResult(IEnumerable<SpecificationError> errors)
{
    public bool IsValid => errors?.Any() != true;
    public IEnumerable<SpecificationError> Errors => errors;
}

