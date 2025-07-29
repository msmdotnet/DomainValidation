namespace DomainValidation.Tests.DomainSpecificationsValidatorTests;
public class DomainSpecificationsValidatorTests
{
    [Theory]
    [MemberData(nameof(TestData.GetTestData),
        MemberType = typeof(TestData))]
    public async Task ValidateAsync_ShouldReturnExpectedResult_WhenValidateDomainSpecifications(
        IDomainSpecification<UserRegistration>[] specifications,
        UserRegistration user,
        ValidationResult expectedResult)
    {
        // Arrange
        IDomainSpecificationsValidator<UserRegistration> Validator =
            new DomainSpecificationsValidator<UserRegistration>(specifications);

        // Act
        var Result = await Validator.ValidateAsync(user);

        // Arrange
        Assert.Equal(expectedResult.IsValid, Result.IsValid);

        if (Result.IsValid == false)
        {
            var ExpectedErrorsOrdered =
                expectedResult.Errors.OrderBy(e => e.PropertyName)
                    .ThenBy(e => e.ErrorMessage);
            var ActualErrorsOrdered =
                Result.Errors.OrderBy(e => e.PropertyName)
                    .ThenBy(e => e.ErrorMessage);

            Assert.Collection(
                ActualErrorsOrdered,
                ExpectedErrorsOrdered
                .Select(expected => (Action<SpecificationError>)(actual =>
                {
                    Assert.Equal(expected.PropertyName, actual.PropertyName);
                    Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
                })
                ).ToArray()
            );
        }
        else
        {
            Assert.True(Result.Errors == null || !Result.Errors.Any());
        }
    }
}

