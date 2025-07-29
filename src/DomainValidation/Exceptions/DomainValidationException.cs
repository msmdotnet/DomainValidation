namespace DomainValidation.Exceptions;
public class DomainValidationException : Exception
{
    public IReadOnlyList<SpecificationError> Errors { get; }
    public DomainValidationException() { }
    public DomainValidationException(string message) : base(message) { }
    public DomainValidationException(string message, Exception innerException)
        : base(message, innerException) { }
    public DomainValidationException(
        IEnumerable<SpecificationError> errors, string message = default)
        : base(message)
    {
        Errors = [.. errors];
    }
}
