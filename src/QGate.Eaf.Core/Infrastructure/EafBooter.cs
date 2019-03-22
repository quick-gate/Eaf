using QGate.Eaf.Core.Metadatas.Services;
using QGate.Eaf.Domain.Metadatas.Models;
using System.Linq;
using System.Reflection;

namespace QGate.Eaf.Core.Infrastructure
{
    public class EafBooter
    {
        public void Boot(params Assembly[] metadataAssemblies)
        {
            //TODO move registration to shared part dependency config for all platforms
            Domain.Infrastructure.Ioc.ServiceLocator.EntityDescriptorFactory = new EntityDescriptorFactory();

            var entityDescriptorType = typeof(IEntityDescriptor);
            foreach (var assembly in metadataAssemblies)
            {
                var descriptorTypes = assembly.GetExportedTypes().Where(x => entityDescriptorType.IsAssignableFrom(x) && !x.IsAbstract);
                foreach (var desriptorType in descriptorTypes)
                {
                    desriptorType
                        .GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .GetValue(null);
                }
            }
        }
    }
}
