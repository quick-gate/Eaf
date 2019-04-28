using Microsoft.AspNetCore.Mvc;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Services;
using System.Collections.Generic;

namespace QGate.Eaf.AspNetCore.Controllers
{
    [Route(BaseUrl + "metadatas")]
    public class MetadataController : ControllerBase
    {
        private readonly IMetadataService _metadataService;

        public MetadataController(IMetadataService entityService)
        {
            _metadataService = entityService;
        }

        [Route("get-entity-list")]
        [HttpPost]
        public IEnumerable<EntityMetadata> GetEntityMetadataList()
        {
            return _metadataService.GetEntityMetadatas();
        }

    }
}
