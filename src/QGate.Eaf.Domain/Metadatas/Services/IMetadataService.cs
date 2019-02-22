using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Models.Params;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Services
{
    public interface IMetadataService
    {
        EntityMetadata GetEntityMetadata(GetEntityMetadataParams parameters);

        IEnumerable<EntityMetadata> GetEntityMetadatas();
    }
}
