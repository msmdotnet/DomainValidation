namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class MatchesTests
{
    Expression<Func<CreateOrder, string>> PropertyExpression =
        x => x.CustomerId;

    [Theory]
    [InlineData(null, "^([0-9]{5})$", false)]
    [InlineData("", "^([0-9]{5})$", false)]
    [InlineData("123", "^([0-9]{5})$", false)]
    [InlineData("123456", "^([0-9]{5})$", false)]
    [InlineData("12345", "^([0-9]{5})$", true)]
    public void Matches_ShouldReturnExpectedResult_WhenValueIsChecked(
        string customerId, string regularExpression, bool expectedResult)
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        Tree.Matches(regularExpression);

        var Entity = new CreateOrder { CustomerId = customerId };

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
    public void Matches_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedErrorMessage = CreateOrder.CustomerIdMatch;
        string RegularExpression = "^([0-9]{5})$";

        Tree.Matches(RegularExpression, ExpectedErrorMessage);

        var Entity = new CreateOrder { CustomerId = "" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void Matches_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string RegularExpression = "^([0-9]{5})$";
        string ExpectedErrorMessage =
            string.Format(ErrorMessages.Matches, RegularExpression);

        Tree.Matches(RegularExpression);

        var Entity = new CreateOrder { CustomerId = "" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

