using QGate.Eaf.Core.Infrastructure;
using QGate.Eaf.Data.Ef;
using QGate.Eaf.Domain.Components.Entities;
using QGate.Eaf.Domain.Entities.Models.Params;
using QGate.Eaf.Domain.Entities.Services;
using QGate.Eaf.Domain.Exceptions;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace QGate.Eaf.Core.Entities.Services
{
    public class EntityService : IEntityService
    {
        private readonly EafDataContext _dataContext;
        public EntityService(EafDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public GetEntityListResult GetEntityList(GetEntityListParams parameters)
        {
            var entityMetadata = ServiceLocator.MetadataService.GetEntityMetadata(new Domain.Metadatas.Models.Params.GetEntityMetadataParams
            {
                EntityName = parameters.EntityName
            });

            if(entityMetadata == null)
            {
                throw new EafException($"Cannot find entity {parameters.EntityName}. Probably has not configured metadata");
            }

            //var entities = _dataContext.Set(entityMetadata.Type, "Description")
            var entities = _dataContext.Set(entityMetadata.Type, "Description")
                //.Where("Code == @0", "001.00027013")
                //.Select("new(Id as XXX, Code)")
                .Take(10)
                
                .ToDynamicList();

            var result = new GetEntityListResult
            {
                EntityList = new EntityList
                {
                    Entities = entities
                }
            };

            return result;
            
        }
    }
}
