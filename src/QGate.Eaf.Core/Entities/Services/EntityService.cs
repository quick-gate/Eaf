using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using QGate.Core.Collections;
using QGate.Core.Exceptions;
using QGate.Eaf.Data.Ef;
using QGate.Eaf.Domain.Components.Editors;
using QGate.Eaf.Domain.Components.Entities;
using QGate.Eaf.Domain.Components.General;
using QGate.Eaf.Domain.Entities.Models.Params;
using QGate.Eaf.Domain.Entities.Services;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Models.Params;
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

        public GetEntityDetailResult GetEntityDetail(GetEntityDetailParams parameters)
        {
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfNullOrEmpty(parameters.Keys, nameof(parameters), nameof(parameters.Keys));

            var entityMetadata = GetEntityMetadata(parameters);

            var query = _dataContext.Set(entityMetadata.Type);
            //foreach (var key in parameters.Keys)
            //{
            //    query = query.Where(key.Name, key.Value);
            //}

            for (int i = 0; i < parameters.Keys.Count; i++)
            {
                query = query.Where(string.Concat(parameters.Keys[i].Name, "=@", i), parameters.Keys[i].Value);
            }

            var entity = query.FirstOrDefault();
            return new GetEntityDetailResult
            {
                EntityDetail = new EntityDetail
                {
                    Entity = entity,
                    Components = new List<ComponentBase>
                    {
                        new TextBox
                        {
                            Binding = new ComponentBinding
                            {
                                PropertyPath = new List<string> { "Code" }
                            }
                        },
                        new TextBox
                        {
                            Binding = new ComponentBinding
                            {
                                PropertyPath = new List<string> { "Name" }
                            }
                        }
                    }
                }
            };
        }

        public GetEntityListResult GetEntityList(GetEntityListParams parameters)
        {
            var entityMetadata = GetEntityMetadata(parameters);

            var entityListAttributes = new List<EntityListAttribute>();

            foreach (var attribute in entityMetadata.Attributes)
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

        public SaveEntityResult SaveEntity(SaveEntityParams parameters)
        {
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfNull(parameters.Entity, nameof(parameters), nameof(parameters.Entity));

            var entityMetadata = _metadataService.GetEntityMetadata(new GetEntityMetadataParams
            {
                EntityName = parameters.EntityName
            });

            var requestEntityType = ((object)parameters.Entity).GetType();

            dynamic convertedEntity = null;
            if (requestEntityType == entityMetadata.Type)
            {
                convertedEntity = parameters.Entity;
            }
            else if (requestEntityType == typeof(JObject))
            {
                convertedEntity = ((JObject)parameters.Entity).ToObject(entityMetadata.Type);
            }
            else
            {
                throw new EafException($"Cannot convert type {requestEntityType.Name} to {entityMetadata.Type.Name} ");
            }

            if (parameters.IsNew)
            {
                _dataContext.Add(convertedEntity);
            }
            else
            {
                var entry = _dataContext.Attach(convertedEntity);
                entry.State = EntityState.Modified;
            }

            _dataContext.SaveChanges();

            return new SaveEntityResult();
        }

        private EntityMetadata GetEntityMetadata(EntityParamsBase parameters)
        {
            var entityMetadata = _metadataService.GetEntityMetadata(new Domain.Metadatas.Models.Params.GetEntityMetadataParams
            {
                EntityName = parameters.EntityName
            });

            if (entityMetadata == null)
            {
                throw new EafException($"Cannot find entity {parameters.EntityName}. Probably has not configured metadata");
            }

            return entityMetadata;
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
