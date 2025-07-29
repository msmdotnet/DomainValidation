namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class GreaterThanExtension
{
    public static PropertySpecificationsTree<T, TProperty>
        GreaterThan<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        TProperty comparisonValue,
        string errorMessage = default) where TProperty : IComparable<TProperty>
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            var ComparableValue =
                (TProperty)Convert.ChangeType(value, typeof(TProperty));

            if (ComparableValue.CompareTo(comparisonValue) <= 0)
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ??
                    string.Format(ErrorMessages.GreaterThan, comparisonValue)));
            }
        })));

        return tree;
    }
}

