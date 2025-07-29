namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class IsRequiredExtension
{
    public static PropertySpecificationsTree<T, TProperty>
        IsRequired<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        string errorMessage = default)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value == null ||
                value is string str && string.IsNullOrWhiteSpace(str))
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ?? ErrorMessages.IsRequired));
            }
        })));

        return tree;
    }
}

