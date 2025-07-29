namespace DomainValidation;
public static class DependencyContainer
{
    public static IServiceCollection AddDomainSpecificationsValidator(
        this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IDomainSpecificationsValidator<>),
            typeof(DomainSpecificationsValidator<>));

        return services;
    }
}


