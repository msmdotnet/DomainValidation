namespace DomainValidation.Tests.SetValidatorTests;
public class SetValidatorTests
{
    [Theory]
    [MemberData(nameof(SetValidatorTestData.GetTestData),
    MemberType = typeof(SetValidatorTestData))]
    public async Task ValidateAsync_ShouldReturnExpectedResult_WhenValidateDomainSpecifications(
        CreateOrder order,
        ValidationResult expectedResult)
    {
        // Arrange
        IDomainSpecificationsValidator<CreateOrderDetail> OrderDetailValidator =
            new DomainSpecificationsValidator<CreateOrderDetail>([
                new CreateOrderDetailSpecification()
                ]);

        IDomainSpecificationsValidator<CreateOrder> Validator =
            new DomainSpecificationsValidator<CreateOrder>([
                new CreateOrderSpecification(OrderDetailValidator)
                ]);

        // Act
        var Result = await Validator.ValidateAsync(order);

        // Assert
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

