using QGate.Core.Collections;
using QGate.Eaf.Data.Ef;
using QGate.Eaf.Domain.Components.Entities;
using QGate.Eaf.Domain.Entities.Models.Params;
using QGate.Eaf.Domain.Entities.Services;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Services;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace QGate.Eaf.Core.Entities.Services
{
    public class EntityService : IEntityService
    {
        private readonly EafDataContext _dataContext;
        private readonly IMetadataService _metadataService;
        public EntityService(EafDataContext dataContext, IMetadataService metadataService)
        {
            _dataContext = dataContext;
            _metadataService = metadataService;
        }
        public GetEntityListResult GetEntityList(GetEntityListParams parameters)
        {
            var entityMetadata = _metadataService.GetEntityMetadata(new Domain.Metadatas.Models.Params.GetEntityMetadataParams
            {
                EntityName = parameters.EntityName
            });

            if(entityMetadata == null)
            {
                throw new EafException($"Cannot find entity {parameters.EntityName}. Probably has not configured metadata");
            }

            var entityListAttributes = new List<EntityListAttribute>();

            foreach (var attribute in entityMetadata.Attributes.Values)
            {
                entityListAttributes.Add(MapEntityListAttribute(attribute));
            }

            //var entities = _dataContext.Set(entityMetadata.Type, "Description")
            var entities = _dataContext.Set(entityMetadata.Type, "Description.Translations")
                //.Where("Description !=nullCode == @0", "001.00027013")
                .Where("Description !=null")
                //.Select("new(Id as XXX, Code)")
                .Take(10)
                
                .ToDynamicList();

            var result = new GetEntityListResult
            {
                EntityList = new EntityList
                {
                    Entities = entities,
                    Attributes = entityListAttributes
                }
            };

            return result;
            
        }

        private EntityListAttribute MapEntityListAttribute(AttributeMetadata attribute)
        {
            return new EntityListAttribute
            {
                Name = attribute.Name,
                Caption = attribute.Translations.IsNullOrEmpty() ?
                    attribute.Name : attribute.Translations[0].Name,
                IsKey = attribute.IsKey
            };
        }
    }
}
