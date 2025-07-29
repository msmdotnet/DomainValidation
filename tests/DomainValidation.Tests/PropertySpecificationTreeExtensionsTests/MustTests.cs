namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class MustTests
{
    Expression<Func<UserRegistration, string>> PropertyExpression =
        (x => x.Password);

    [Theory]
    [InlineData("", false)]
    [InlineData("12345", false)]
    [InlineData("M", true)]
    public void Must_ShouldReturnExpectedResult_WhenValueIsChecked(
        string passwordValue, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            PropertyExpression);
        Tree.Must(p => p.Any(c => char.IsUpper(c)),
            UserRegistration.UppercaseCharactersAreRequiredErrorMessage);

        var Entity = new UserRegistration { Password = passwordValue };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.Equal(expectedResult, Result);

        if (!expectedResult)
        {
            Assert.Single(Tree.Specifications[0].Errors);
        }
        else
        {
            Assert.True(Tree.Specifications[0].Errors == null ||
                !Tree.Specifications[0].Errors.Any());
        }
    }

    [Theory]
    [InlineData("Admin", "Admin", false)]
    [InlineData("Admin@name.com", "Password", true)]
    public void Must_ShouldReturnExpectedResult_WhenEntityIsChecked(
        string email, string password, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            PropertyExpression);

        Tree.Must((UserRegistration t) =>
            t.Email == "Admin@name.com" && t.Password == "Password",
            "Error");

        var Entity = new UserRegistration
        {
            Email = email,
            Password = password
        };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.Equal(expectedResult, Result);

        if (!expectedResult)
        {
            Assert.Single(Tree.Specifications[0].Errors);
        }
        else
        {
            Assert.True(Tree.Specifications[0].Errors == null ||
                !Tree.Specifications[0].Errors.Any());
        }
    }

    [Fact]
    public void Must_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            PropertyExpression);

        string ExpectedMessage =
            UserRegistration.UppercaseCharactersAreRequiredErrorMessage;

        Tree.Must(p => p.Any(c => char.IsUpper(c)), ExpectedMessage);

        var Entity = new UserRegistration { Password = "12345" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

