namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class NotEmptyTests
{
    static Expression<Func<CreateOrder, IEnumerable<CreateOrderDetail>>>
        PropertyExpression = x => x.OrderDetails;

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void NotEmpty_ShouldReturnExpectedResult_WhenValueIsChecked(
        IEnumerable<CreateOrderDetail> details, bool expectedResult)
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder,
            IEnumerable<CreateOrderDetail>>(PropertyExpression);

        Tree.NotEmpty();

        var Entity = new CreateOrder { OrderDetails = details };

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
    public void NotEmpty_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder,
            IEnumerable<CreateOrderDetail>>(PropertyExpression);
        string ExpectedErrorMessage = CreateOrder.NotEmpty;

        Tree.NotEmpty(ExpectedErrorMessage);

        var Entity = new CreateOrder { OrderDetails = null };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void NotEmpty_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder,
            IEnumerable<CreateOrderDetail>>(PropertyExpression);
        string ExpectedErrorMessage = ErrorMessages.NotEmpty;

        Tree.NotEmpty();

        var Entity = new CreateOrder { OrderDetails = null };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[]
        {
            null, false
        };

        yield return new object[]
        {
            new List<CreateOrderDetail>(), false
        };

        yield return new object[]
        {
            new List<CreateOrderDetail>(){new() }, true
        };
    }
}

