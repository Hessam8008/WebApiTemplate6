using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            /* Add MediateR */
            services.AddMediatR(AssemblyReference.Assembly);


            return services;
        }
    }
}
