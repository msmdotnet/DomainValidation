namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static partial class EmailAddressExtension
{
    public static PropertySpecificationsTree<T, string> EmailAddress<T>(
        this PropertySpecificationsTree<T, string> tree,
        string errorMessage = default)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (string.IsNullOrWhiteSpace(value) ||
                !EmailRegex().IsMatch(value))
            {
                errors.Add(new SpecificationError(
                    tree.PropertyName, errorMessage ??
                    ErrorMessages.EmailAddress));
            }
        })));

        return tree;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}

