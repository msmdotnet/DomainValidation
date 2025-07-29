namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class EqualExtension
{
    public static PropertySpecificationsTree<T, TProperty> Equal<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        TProperty comparisonValue,
        string errorMessage = default) where TProperty : IComparable<TProperty>
    {
        AddSpecification(tree, (entity) => comparisonValue, errorMessage);

        return tree;
    }

    public static PropertySpecificationsTree<T, TProperty> Equal<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        Expression<Func<T, TProperty>> ComparisonProperty,
        string errorMessage = default) where TProperty : IComparable<TProperty>
    {
        AddSpecification(tree, (entity) =>
            ComparisonProperty.Compile().Invoke(entity), errorMessage);

        return tree;
    }

    static void AddSpecification<T, TProperty>(
        PropertySpecificationsTree<T, TProperty> tree,
        Func<T, TProperty> getcomparisonValue,
        string errorMessage) where TProperty : IComparable<TProperty>
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            TProperty ComparisonValue = getcomparisonValue(entity);
            bool AddError;

            if (value == null && ComparisonValue == null)
            {
                AddError = false;
            }
            else if (value == null || ComparisonValue == null)
            {
                AddError = true;
            }
            else
            {
                // Evitar conversión innecesaria si value ya es del tipo correcto
                TProperty ComparableValue = value is TProperty TPropertyValue
                    ? TPropertyValue
                    : (TProperty)Convert.ChangeType(value, typeof(TProperty));

                AddError = ComparableValue.CompareTo(ComparisonValue) != 0;
            }

            if (AddError)
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ?? ErrorMessages.Equal));
            }
        })));
    }
}

