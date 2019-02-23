using QGate.Eaf.Core.Infrastructure;
using QGate.Eaf.Data.Ef;
using System.Reflection;

namespace QGate.Eaf.Application.Infrastructure.Configuration
{
    public class EafAppConfig
    {
        private readonly EafDependencyConfig _ependencyConfig;

        public EafAppConfig(EafDependencyConfig ependencyConfig)
        {
            _ependencyConfig = ependencyConfig;
        }

        public EafAppConfig AddDataContext<TDataContext>(string connectionString) where TDataContext : EafDataContext
        {
            _ependencyConfig.AddDataContext<TDataContext>(connectionString);
            return this;
        }

        /// <summary>
        /// Add descriptor metadata assemblies
        /// </summary>
        /// <param name="metadataAssemblies"></param>
        /// <returns></returns>
        public EafAppConfig AddMetadataAssemblies(params Assembly[] metadataAssemblies)
        {
            new EafBooter().Boot(metadataAssemblies);
            return this;
        }
    }
}
