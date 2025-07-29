﻿namespace DomainValidation.Tests.DomainSpecificationsTests;
public class PropertyStopOnFirstErrorTests
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
            new PropertyWithoutStopOnFirstErrorSpecification(),
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
            new PropertyWithoutStopOnFirstErrorSpecification(),
            new UserRegistration(){Password = ""},
            false,
            new List<SpecificationError>() {
                new SpecificationError("Password",
                    UserRegistration.HasMinLengthErrorMessage),
                new SpecificationError("Password",
                    UserRegistration.UppercaseCharactersAreRequiredErrorMessage),
                new SpecificationError("Password",
                    UserRegistration.LowercaseCharactersAreRequiredErrorMessage),
                new SpecificationError("Password",
                    UserRegistration.DigitsAreRequiredErrorMessage),
                new SpecificationError("Password",
                    UserRegistration.SymbolsAreRequiredErrorMessage)
            }
        };

        yield return new object[]
        {
            new PropertyWithStopOnFirstErrorSpecification(),
            new UserRegistration()
            {
                Email = "name@hotmail.com",
                Password = "Mm1234567$",
                ConfirmPassword="Mm1234567$"},
            true,
            new List<SpecificationError>()
        };

        yield return new object[]
        {
            new PropertyWithStopOnFirstErrorSpecification(),
            new UserRegistration(){Password = "1234567"},
            false,
            new List<SpecificationError>() {
                new SpecificationError("Password",
                UserRegistration.UppercaseCharactersAreRequiredErrorMessage)
            }
        };

        yield return new object[]
        {
            new PropertyWithStopOnFirstErrorSpecification(),
            new UserRegistration(){Password = "MmABCDEF"},
            false,
            new List<SpecificationError>() {
                new SpecificationError("Password",
                UserRegistration.DigitsAreRequiredErrorMessage)
            }
        };
    }
}

