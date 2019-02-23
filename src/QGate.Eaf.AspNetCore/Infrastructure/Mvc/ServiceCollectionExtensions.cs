using Microsoft.Extensions.DependencyInjection;
using QGate.Eaf.Application.Infrastructure.Configuration;
using QGate.Eaf.Core.Infrastructure;

namespace QGate.Eaf.AspNetCore.Infrastructure.Mvc
{
    public static class ServiceCollectionExtensions
    {
        
        public static EafAppConfig AddEaf(this IServiceCollection services)
        {
            return new EafAppConfig(
                new EafDependencyConfig(services).AddServices());
        }

    }
}
