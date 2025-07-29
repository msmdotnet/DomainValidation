namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class MatchesExtension
{
    public static PropertySpecificationsTree<T, string> Matches<T>(
        this PropertySpecificationsTree<T, string> tree,
        string regularExpression, string errorMessage = default)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value == null || !Regex.IsMatch(value, regularExpression))
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ??
                    string.Format(ErrorMessages.Matches, regularExpression)));
            }
        })));

        return tree;
    }
}

