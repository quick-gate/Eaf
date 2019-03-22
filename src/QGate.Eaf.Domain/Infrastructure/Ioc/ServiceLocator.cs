using QGate.Eaf.Domain.Metadatas.Services;

namespace QGate.Eaf.Domain.Infrastructure.Ioc
{
    public static class ServiceLocator
    {
        public static IEntityDescriptorFactory EntityDescriptorFactory { get; set; }
    }
}
