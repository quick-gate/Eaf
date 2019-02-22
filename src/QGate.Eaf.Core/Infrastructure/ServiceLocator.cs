using QGate.Eaf.Core.Metadatas.Services;
using QGate.Eaf.Domain.Metadatas.Services;

namespace QGate.Eaf.Core.Infrastructure
{
    public class ServiceLocator
    {
        private static IMetadataService _metadataService;
        public static IMetadataService MetadataService
        {
            get
            {
                if (_metadataService == null)
                {
                    _metadataService = new MetadataService();
                }

                return _metadataService;
            }
        }
    }
}
