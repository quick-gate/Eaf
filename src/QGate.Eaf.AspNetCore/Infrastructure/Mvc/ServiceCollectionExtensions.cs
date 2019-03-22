using Microsoft.Extensions.DependencyInjection;
using QGate.Eaf.Application.Infrastructure.Configuration;
using QGate.Eaf.AspNetCore.Controllers;

namespace QGate.Eaf.AspNetCore.Infrastructure.Mvc
{
    public static class ServiceCollectionExtensions
    {
        
        public static EafAppConfig AddEaf(this IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(typeof(EntityController).Assembly)
                .AddControllersAsServices();

            return new EafAppConfig(
                new EafDependencyConfig(services).AddServices());
        }

    }
}
