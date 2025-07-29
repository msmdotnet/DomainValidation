namespace DomainValidation.Tests.DomainSpecificationsValidatorTests;
internal class ConditionalUniqueEmailSpecification :
    DomainSpecificationBase<UserRegistration>
{
    public const string DuplicateEmailMessage =
        "The email provided already exists.";
    public const string TestEmail = "user@northwind.com";
    public ConditionalUniqueEmailSpecification() : base(true) { }

    protected override async Task<List<SpecificationError>>
        ValidateSpecificationsAsync(
        UserRegistration entity)
    {
        List<SpecificationError> Errors = [];

        // Simular una operación asíncrona, por ejemplo,
        // una búsqueda en una base de datos
        await Task.Delay(1000);
        bool DuplicateEmail = TestEmail.Equals(entity.Email);

        if (DuplicateEmail)
            Errors.Add(new SpecificationError("Email", DuplicateEmailMessage));

        return Errors;
    }
}

