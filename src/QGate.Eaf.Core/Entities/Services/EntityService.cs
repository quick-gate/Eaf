using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json.Linq;
using QGate.Core.Collections;
using QGate.Core.Exceptions;
using QGate.Eaf.Data.Ef;
using QGate.Eaf.Domain.Components.Editors;
using QGate.Eaf.Domain.Components.Entities;
using QGate.Eaf.Domain.Components.General;
using QGate.Eaf.Domain.Entities.Models;
using QGate.Eaf.Domain.Entities.Models.Params;
using QGate.Eaf.Domain.Entities.Services;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Models.Params;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

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

            var entityMetadata = GetEntityMetadata(parameters);

            dynamic entity;

            if (parameters.Keys.IsNullOrEmpty())
            {
                entity = Activator.CreateInstance(entityMetadata.Type);
            }
            else
            {
                var query = _dataContext.Set(
                    entityMetadata.Type,
                    parameters.IncludePropertyPaths.IsNullOrEmpty() ? null : parameters.IncludePropertyPaths.ToArray());

                query = AddKeyConditions(query, parameters.Keys);
                entity = query.FirstOrDefault();

            }

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

        private IQueryable AddKeyConditions(IQueryable query, IList<AttributeValue> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                query = query.Where(string.Concat(keys[i].Name, "=@", i), keys[i].Value);
            }

            return query;
        }

        public GetEntityListResult GetEntityList(GetEntityListParams parameters)
        {
            var entityMetadata = GetEntityMetadata(parameters);

            var entityListAttributes = new List<EntityListAttribute>();

            foreach (var attribute in entityMetadata.Attributes)
            {
                entityListAttributes.Add(MapEntityListAttribute(attribute));
            }


            List<dynamic> entities = GetEntityListQuery(entityMetadata)
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

        private IQueryable GetEntityListQuery(EntityMetadata entityMetadata)
        {
            StringBuilder selectDefinition = null;

            foreach (var attribute in entityMetadata.Attributes)
            {
                if (selectDefinition == null)
                {
                    selectDefinition = new StringBuilder("new(");
                }
                else
                {
                    selectDefinition.Append(",");
                }

                selectDefinition.Append(attribute.Name);
            }

            selectDefinition.Append(")");



            //var entities = _dataContext.Set(entityMetadata.Type, "Description")
            return _dataContext.Set(entityMetadata.Type)
                //.Where("Description !=nullCode == @0", "001.00027013")
                //.Where("Description !=null")
                .Select(selectDefinition.ToString())
                //.Select("new(Id as XXX, Code)")
                .Take(10);
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

            var result = new SaveEntityResult();

            if (parameters.IsFillEntityListItemRequired)
            {
                var entityEntry = _dataContext.Entry(convertedEntity);
                var keys = new List<AttributeValue>();
                foreach (var key in entityMetadata.GetKeys())
                {
                    keys.Add(new AttributeValue(key.Name, entityEntry.Property(key.Name).CurrentValue));
                }

                var query = GetEntityListQuery(entityMetadata);
                query = AddKeyConditions(query, keys);
                result.EntityLisItem = query.FirstOrDefault();
            }

            return result;
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
