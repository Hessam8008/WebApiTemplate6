namespace Application;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Dependencies injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     The add application layer services.
    /// </summary>
    /// <param name="services">
    ///     The services.
    /// </param>
    /// <returns>
    ///     The <see cref="IServiceCollection" />.
    /// </returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        /* Add MediateR */
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly));

        return services;
    }
}