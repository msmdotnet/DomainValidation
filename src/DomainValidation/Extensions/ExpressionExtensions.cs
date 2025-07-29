namespace DomainValidation.Extensions;
internal static class ExpressionExtensions
{
    public static string GetPropertyName<T, TProperty>(
        this Expression<Func<T, TProperty>> propertyExpression)
    {
        string PropertyName = null;

        var Body = propertyExpression.Body;

        if (Body is UnaryExpression UnaryExpression)
        {
            Body = UnaryExpression.Operand;
        }

        if (Body is MemberExpression MemberExpression)
        {
            PropertyName = MemberExpression.Member.Name;
        }

        return PropertyName;
    }
}

