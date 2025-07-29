namespace DomainValidation.Tests.DomainSpecificationsTests;
public class SpecificationStopOnFirstErrorTests
{
    [Theory]
    [MemberData(nameof(GetTestData))]
    public async Task ValidateAsync_ShouldReturnExpectedResult_WhenValidate(
        IDomainSpecification<UserRegistration> specification,
        UserRegistration entity,
        bool expectedResult,
        IEnumerable<SpecificationError> expectedErrors)
    {
        // Act
        var Result = await specification.ValidateAsync(entity);

        // Assert
        Assert.Equal(expectedResult, Result);

        if (Result == false)
        {
            var ExpectedErrorsOrdered =
                expectedErrors.OrderBy(e => e.PropertyName)
                .ThenBy(e => e.ErrorMessage);
            var ActualErrorsOrdered =
                specification.Errors.OrderBy(e => e.PropertyName)
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
            Assert.True(specification.Errors == null ||
                !specification.Errors.Any());
        }
    }

    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[]
        {
            new SpecificationWithoutStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "name@hotmail.com",
                Password = "Mm1234567$",
                ConfirmPassword="Mm1234567$"
            },
            true,
            new List<SpecificationError>()
        };

        yield return new object[]
        {
            new SpecificationWithoutStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "",
                Password = "123",
                ConfirmPassword = "456"
            },
            false,
            new List<SpecificationError>()
            {
                new SpecificationError("Email",
                    UserRegistration.IsRequiredErrorMessage),
                new SpecificationError("Password",
                    UserRegistration.HasMinLengthErrorMessage),
                new SpecificationError("ConfirmPassword",
                  UserRegistration.PasswordConfirmationDoesNotMatchErrorMessage),
            }
        };

        yield return new object[]
        {
            new SpecificationWithStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "name@hotmail.com",
                Password = "Mm1234567$",
                ConfirmPassword="Mm1234567$"
            },
            true,
            new List<SpecificationError>()
        };

        yield return new object[]
        {
            new SpecificationWithStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "",
                Password = "123",
                ConfirmPassword="456"
            },
            false,
            new List<SpecificationError>()
            {
                new SpecificationError("Email",
                UserRegistration.IsRequiredErrorMessage),
            }
        };

        yield return new object[]
        {
            new SpecificationWithStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "name@hotmail.com",
                Password = "123",
                ConfirmPassword="456"
            },
            false,
            new List<SpecificationError>()
            {
                new SpecificationError("Password",
                    UserRegistration.HasMinLengthErrorMessage),
            }
        };

        yield return new object[]
        {
            new SpecificationWithStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "name@hotmail.com",
                Password = "Mm1234567$",
                ConfirmPassword="$Mm1234567"
            },
            false,
            new List<SpecificationError>()
            {
                new SpecificationError("ConfirmPassword",
                  UserRegistration.PasswordConfirmationDoesNotMatchErrorMessage),
            }
        };
    }
}

