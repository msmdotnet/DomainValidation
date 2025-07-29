namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class HasMaxLengthExtension
{
    public static PropertySpecificationsTree<T, string> HasMaxLength<T>(
        this PropertySpecificationsTree<T, string> tree,
        int maxLength, string errorMessage = default)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value != null && value.Length > maxLength)
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ??
                    string.Format(ErrorMessages.HasMaxLength, maxLength)));
            }
        })));

        return tree;
    }
}

