using QGate.Core.Collections;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Globalization;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Models.Params;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QGate.Core.Types;
namespace QGate.Eaf.Core.Metadatas.Services
{
    public class MetadataService : IMetadataService
    {
        private static IDictionary<string, EntityMetadata> _entityMetadataDictionary = new Dictionary<string, EntityMetadata>();
        private IList<RelationInfo> _relationInfos = new List<RelationInfo>();
        private readonly Type AttributeMetadataType = typeof(AttributeMetadata);
        //private Type RelationMetadataType = typeof(RelationMetadata);
        private readonly Type EntityDescriptorType = typeof(EntityDescriptor);
        //TODO add to configuration
        private readonly string _defaultLanguage = LanguageCodes.en;

        private static bool _isInitialized;

        private void Init()
        {
            if (_isInitialized)
            {
                return;
            }

            var descriptors = EntityDescriptor.GetDescriptors();

            _isInitialized = true;

            if (descriptors == null)
            {
                return;
            }

            foreach (var descriptor in descriptors)
            {
                MapDescriptorToMetadata(descriptor);
            }

            foreach (var referenceInfo in _relationInfos)
            {
                referenceInfo.RelationReference.Entity = referenceInfo.Relation.Owner;

                if (referenceInfo.Relation.RelationType == RelationType.OneToOne)
                {
                    referenceInfo.RelationReference.Attributes = GetRelationReferenceAttributes(referenceInfo.Relation);
                    referenceInfo.RelationReference.IsComposition = true;
                    if (referenceInfo.RelationReference.Type.IsCollection())
                    {
                        referenceInfo.RelationReference.RelationType = RelationType.OneToMany;
                    }
                    else
                    {
                        referenceInfo.RelationReference.RelationType = RelationType.OneToOneInverted;
                    }
                }
            }
        }


        /// <summary>
        /// Inverts relation attributes
        /// </summary>
        /// <param name="relationAttributes"></param>
        /// <returns></returns>
        private IList<RelationAttribute> GetRelationReferenceAttributes(RelationMetadata relation)
        {
            return relation.Attributes.Select(
                            x =>
                            {
                                return new RelationAttribute(x.LinkedAttribute, x.Attribute);
                            }).ToList();
        }

        private void MapDescriptorToMetadata(EntityDescriptor descriptor)
        {
            var descriptorType = descriptor.GetType();

            var entityMetadata = descriptor.Entity;

            if (entityMetadata.Translations.IsNullOrEmpty())
            {
                entityMetadata.Translations = new List<MetadataTranslation>
                {
                    new MetadataTranslation(entityMetadata.Type.Name, _defaultLanguage)
                };
            }

            _entityMetadataDictionary.Add(entityMetadata.Name, entityMetadata);

            foreach (var propertyInfo in descriptorType.GetProperties())
            {
                var attributeName = propertyInfo.Name;
                if (propertyInfo.PropertyType == AttributeMetadataType)
                {
                    var attributeMetadata = GetMetadata<AttributeMetadata>(propertyInfo, descriptor, descriptorType);

                    var targetProperty = SetAttributeNameAndGetTargetProperty(entityMetadata, attributeName, attributeMetadata);

                    if (targetProperty == null)
                    {
                        throw new EafException($"EntityDescriptor {descriptorType.Name} initialization failed. Cannot find attribute {propertyInfo.Name} in {entityMetadata.Type.FullName}.");
                    }

                    attributeMetadata.Type = targetProperty.PropertyType;

                    if (attributeMetadata.AttributeType == null)
                    {
                        attributeMetadata.AttributeType = new AttributeType();
                    }

                    if (attributeMetadata.AttributeType.DataType == DataType.Unspecified)
                    {
                        if (attributeMetadata.Type == typeof(string))
                        {
                            attributeMetadata.AttributeType.DataType = DataType.String;
                        }
                        else if (attributeMetadata.Type == typeof(decimal))
                        {
                            attributeMetadata.AttributeType.DataType = DataType.Decimal;
                        }
                        else if (attributeMetadata.Type == typeof(int))
                        {
                            attributeMetadata.AttributeType.DataType = DataType.Integer;
                        }
                        else if (attributeMetadata.Type == typeof(bool))
                        {
                            attributeMetadata.AttributeType.DataType = DataType.Boolean;
                        }
                    }
                }
                //else if (RelationMetadataType.IsAssignableFrom(propertyInfo.PropertyType))
                //{
                //    var relationMetadata = GetMetadata<RelationMetadata>(propertyInfo, descriptor, descriptorType);

                //    relationMetadata.Name = propertyInfo.Name;

                //    if(!relationMetadata.IsReference && relationMetadata.EntityReferenceAttribute != null)
                //    {
                //        _referenceRelations.Add((relationMetadata, relationMetadata.EntityReferenceAttribute));
                //    }

                    
                //}
                //else if(typeof(RelationDescriptor).IsAssignableFrom(propertyInfo.PropertyType))
                //{
                //    var relationDescriptor = (RelationDescriptor)propertyInfo.GetValue(descriptor);

                //    var relationMetadata = relationDescriptor.Metadata;
                //    FillAttributeMetadataBase(descriptor.Entity, relationMetadata);

                //    relationMetadata.Name = propertyInfo.Name;

                //    if (!relationMetadata.IsReference && relationMetadata.EntityReferenceAttribute != null)
                //    {
                //        _referenceRelations.Add((relationMetadata, relationMetadata.EntityReferenceAttribute));
                //    }

                //}
                else if (EntityDescriptorType.IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var entityDescriptor = (EntityDescriptor)propertyInfo.GetValue(descriptor);
                    var entityDescriptorContext = (IEntityDescriptorContext)entityDescriptor;
                    var relationDescriptor = entityDescriptorContext.RelationDescriptor;

                    var relationMetadata = relationDescriptor.Metadata;
                    FillAttributeMetadataBase(descriptor.Entity, relationMetadata);

                    relationMetadata.Name = propertyInfo.Name;

                    relationMetadata.RelationType = propertyInfo.PropertyType.IsCollection() ?
                        RelationType.OneToMany : RelationType.OneToOne;
                    

                    var relationInfo = new RelationInfo(relationMetadata);
                    entityMetadata.RelationInfos.Add(relationInfo);

                    relationMetadata.Type = entityMetadata.Type.GetProperty(relationMetadata.Name).PropertyType;

                    if (!relationMetadata.IsReference && relationMetadata.EntityReferenceAttribute != null)
                    {
                        relationInfo.RelationReference = relationMetadata.EntityReferenceAttribute;

                        _relationInfos.Add(relationInfo);
                    }

                }
                //else if (propertyInfo.PropertyType.IsSubclassOf(_relationDescriptorBaseType))
                //{
                //    var relationDescriptor = (RelationDescriptor)propertyInfo.GetValue(this) ??
                //       throw new EafException($"RelationDescriptor {descriptorType.Name} initialization failed. Cannot find relation {propertyInfo.Name} .");

                //    relationDescriptor.DescriptorName = attributeName;
                //    metadata.AddRelation(relationDescriptor.GetMetadata());

                //}
            }
        }

        private void FillAttributeMetadataBase(EntityMetadata entityMetadata, AttributeMetadataBase attributeMetadataBase)
        {
            attributeMetadataBase.Owner = entityMetadata;
            if (attributeMetadataBase.Translations.IsNullOrEmpty())
            {
                attributeMetadataBase.Translations = new List<MetadataTranslation>
                {
                    new MetadataTranslation(attributeMetadataBase.Name, _defaultLanguage)
                };
            }
        }

        private static PropertyInfo SetAttributeNameAndGetTargetProperty(EntityMetadata entityMetadata, string attributeName, AttributeMetadata attributeMetadata)
        {
            attributeMetadata.Name = attributeName;

            var targetProperty = entityMetadata.Type.GetProperty(attributeMetadata.Name);
            return targetProperty;
        }

        public TMetadata GetMetadata<TMetadata>(PropertyInfo property, EntityDescriptor descriptor, Type descriptorType) where TMetadata : AttributeMetadataBase
        {
            var metadata = (TMetadata)property.GetValue(descriptor) ??
                        throw new EafException($"EntityDescriptor {descriptorType.FullName} initialization failed. Cannot find attribute {property.Name} .");

            FillAttributeMetadataBase(descriptor.Entity, metadata);

            return metadata;
        }

        public EntityMetadata GetEntityMetadata(GetEntityMetadataParams parameters)
        {
            Init();

            _entityMetadataDictionary.TryGetValue(parameters.EntityName, out EntityMetadata entityMetadata);
            return entityMetadata;
        }

        public IEnumerable<EntityMetadata> GetEntityMetadatas()
        {
            Init();
            return _entityMetadataDictionary.Values;
        }
    }
}
