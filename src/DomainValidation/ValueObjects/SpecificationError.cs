namespace DomainValidation.ValueObjects;
public class SpecificationError(string propertyName, string errorMessage)
{
    public string PropertyName => propertyName;
    public string ErrorMessage => errorMessage;
}

