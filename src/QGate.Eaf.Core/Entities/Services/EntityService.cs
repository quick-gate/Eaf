using Microsoft.EntityFrameworkCore;
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
        private const string IncludeAllChar = "*";
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

            var entity = GetEntity(entityMetadata, parameters.Keys, parameters.IncludePropertyPaths);

            var result = new GetEntityDetailResult
            {
                EntityDetail = CreateEntityComponentBase<EntityDetail>(entityMetadata, null)
            };

            result.EntityDetail.Entity = entity;
            result.EntityDetail.Components = new List<ComponentBase>();


            foreach (var attribute in entityMetadata.Attributes)
            {
                if (attribute.IsRelationKey)
                {
                    continue;
                }
                result.EntityDetail.Components.Add(FillComponentBase(new TextBox { IsReadOnly = attribute.IsKey }, attribute));
            }

            if (entityMetadata.Relations.IsNullOrEmpty())
            {
                return result;
            }


            foreach (var relationInfo in entityMetadata.RelationInfos)
            {
                var relation = relationInfo.Relation;
                if (relationInfo.RelationReference != null && relationInfo.RelationReference.IsComposition)
                {
                    //Relation in composition will not be rendered because is not allowed change owner
                    continue;
                }

                if (relation.RelationType == RelationType.OneToMany)
                {
                    var entityList = CreateEntityComponentBase<EntityList>(relation.Entity, relation);
                    entityList.Attributes = GetEntityListAttributes(relation.Entity);
                    entityList.RelationAttributes = GetRelationAttributes(relation);
                    entityList.EntityName = relation.Entity.Name;

                    result.EntityDetail.Components.Add(entityList);
                    continue;
                }



                var entitySelector = FillComponentBase(new EntitySelector(), relation);
                entitySelector.EntityName = relation.Entity.Name;
                //TODO define better condition for display attributes
                entitySelector.DisplayAttributes = relation.Entity.Attributes.Select(x => x.Name).Take(2).ToList();
                if (!relation.Attributes.IsNullOrEmpty())
                {
                    entitySelector.RelationAttributes = GetRelationAttributes(relation);
                }
                entitySelector.IsComposition = relation.IsComposition;
                entitySelector.IsInverted = relation.RelationType == RelationType.OneToOneInverted;
                //entitySelector.KeyAttributes = relation.Keys.Select(x => x.Name).ToList();
                result.EntityDetail.Components.Add(entitySelector);

            }




            return result;
        }

        private dynamic GetEntity(EntityMetadata entityMetadata, IList<AttributeValue> keys, IList<string> includes)
        {
            if (includes != null && includes.Count == 1 && includes[0] == IncludeAllChar)
            {
                includes = new List<string>();
                foreach (var relation in entityMetadata.Relations)
                {
                    includes.Add(relation.Name);
                }
            }

            if (keys.IsNullOrEmpty())
            {
                return Activator.CreateInstance(entityMetadata.Type);
            }
            else
            {
                var query = _dataContext.Set(
                    entityMetadata.Type,
                    includes.IsNullOrEmpty() ? null : includes.ToArray());

                query = AddKeyConditions(query, keys);
                return query.FirstOrDefault();

            }
        }

        private TComponent CreateEntityComponentBase<TComponent>(EntityMetadata entityMetadata, MetadataBase relationOrAttribute) where TComponent : EntityComponentBase
        {
            var component = Activator.CreateInstance<TComponent>();
            component.EntityName = entityMetadata.Name;
            //TODO Improve get current language
            component.EntityCaption = entityMetadata.Translations.FirstOrDefault()?.Name;
            if (relationOrAttribute != null)
            {
                FillComponentBase(component, relationOrAttribute);
            }

            return component;
        }

        private IList<RelationAttributeDto> GetRelationAttributes(RelationMetadata relation)
        {
            return relation.Attributes.Select(x => new RelationAttributeDto(x.Attribute.Name, x.LinkedAttribute.Name)).ToList();
        }

        private TComponent FillComponentBase<TComponent>(TComponent component, MetadataBase attribute) where TComponent : ComponentBase
        {
            component.Caption = attribute.Translations?.FirstOrDefault().Name;
            component.Binding = new ComponentBinding { PropertyPath = new List<string> { attribute.Name } };
            return component;
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
            var entityListAttributes = GetEntityListAttributes(entityMetadata);

            var query = GetEntityListQuery(entityMetadata);
            List<dynamic> entities = query.ToDynamicList();

            var entityList = CreateEntityComponentBase<EntityList>(entityMetadata, null);
            entityList.Entities = entities;
            entityList.Attributes = entityListAttributes;
            var result = new GetEntityListResult
            {
                EntityList = entityList
            };

            return result;

        }

        private List<EntityListAttribute> GetEntityListAttributes(EntityMetadata entityMetadata)
        {
            var entityListAttributes = new List<EntityListAttribute>();

            foreach (var attribute in entityMetadata.Attributes)
            {
                entityListAttributes.Add(MapEntityListAttribute(attribute));
            }

            return entityListAttributes;
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
                .Take(1000);


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

        public DeleteEntityResult DeleteEntity(DeleteEntityParams parameters)
        {
            var entityMetadata = GetEntityMetadata(parameters);

            var entity = GetEntity(entityMetadata, parameters.Keys, new List<string> { IncludeAllChar });

            _dataContext.Remove(entity);
            _dataContext.SaveChanges();

            return new DeleteEntityResult();

        }
    }
}
