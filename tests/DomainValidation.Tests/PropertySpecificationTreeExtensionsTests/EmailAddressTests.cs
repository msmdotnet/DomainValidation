namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class EmailAddressTests
{
    Expression<Func<UserRegistration, string>> PropertyExpression =
        x => x.Email;

    [Theory]
    [InlineData(null, false)]
    [InlineData("  ", false)]
    [InlineData("@hotmail.com", false)]
    [InlineData("name@hotmail", false)]
    [InlineData(".com", false)]
    [InlineData("name@hotmail.com", true)]
    public void EmailAddress_ShouldReturnExpectedResult_WhenValueIsChecked(
        string email, bool expectedResult)
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            PropertyExpression);
        Tree.EmailAddress();

        var Entity = new UserRegistration { Email = email };

        // Act
        bool result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.Equal(expectedResult, result);

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
    public void EmailAddress_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            PropertyExpression);
        string ExpectedErrorMessage = UserRegistration.EmailErrorMessage;

        Tree.EmailAddress(ExpectedErrorMessage);

        var Entity = new UserRegistration { Email = "name" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void EmailAddress_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            PropertyExpression);
        string ExpectedErrorMessage = ErrorMessages.EmailAddress;

        Tree.EmailAddress();

        var Entity = new UserRegistration { Email = "name" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

