using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QGate.Core.Exceptions;
using QGate.Eaf.Core.Entities.Services;
using QGate.Eaf.Core.Metadatas.Services;
using QGate.Eaf.Data.Ef;
using QGate.Eaf.Domain.Entities.Services;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Reflection;

namespace QGate.Eaf.Application.Infrastructure.Configuration
{
    public class EafDependencyConfig
    {
        private readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        public EafDependencyConfig(IServiceCollection services)
        {
            _services = services;
            Throw.IfNull(services, nameof(services));
        }

        public EafDependencyConfig AddServices()
        {
            Services.AddTransient<IEntityService, EntityService>();
            Services.AddTransient<IMetadataService, MetadataService>();

            return this;
        }

        public EafDependencyConfig AddDataContext<TDataContext>(string connectionString, Assembly migrationAssembly = null) where TDataContext: EafDataContext
        {
            Services
                .AddSingleton<DbContextOptions>(
                new DbContextOptionsBuilder<TDataContext>()
                    .UseSqlServer(connectionString, x=> 
                    {
                        if(migrationAssembly != null)
                        {
                            x.MigrationsAssembly(migrationAssembly.FullName);
                        }
                    })
                    .Options
                );

            Services.AddTransient<TDataContext>();
            Services.AddTransient<EafDataContext>(x => x.GetService<TDataContext>());

            return this;
        }

        public IServiceProvider Build()
        {
            return Services.BuildServiceProvider();
        }
    }
}
