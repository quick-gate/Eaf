using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Models.Params;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace QGate.Eaf.Core.Metadatas.Services
{
    public class MetadataService : IMetadataService
    {
        private static IDictionary<string, EntityMetadata> _entityMetadataDictionary = new Dictionary<string, EntityMetadata>();
        private Type AttributeMetadataType = typeof(AttributeMetadata);
        private Type RelationMetadataType = typeof(RelationMetadata);

        private static bool _isInitialized;

        private void Init()
        {
            if(_isInitialized)
            {
                return;
            }

            var descriptors = EntityDescriptor.GetDescriptors();

            _isInitialized = true;

            if(descriptors == null)
            {
                return;
            }

            foreach (var descriptor in descriptors)
            {
                MapDescriptorToMetadata(descriptor);
            }
        }


        private void MapDescriptorToMetadata(EntityDescriptor descriptor)
        {
            var descriptorType = descriptor.GetType();


            var entityMetadata = descriptor.Entity;

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
                else if(propertyInfo.PropertyType == RelationMetadataType)
                {
                    var relationMetadata = GetMetadata<RelationMetadata>(propertyInfo, descriptor, descriptorType);

                    relationMetadata.Name = propertyInfo.Name;
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

        private static PropertyInfo SetAttributeNameAndGetTargetProperty(EntityMetadata entityMetadata, string attributeName, AttributeMetadata attributeMetadata)
        {
            attributeMetadata.Name = attributeName;

            var targetProperty = entityMetadata.Type.GetProperty(attributeMetadata.Name);
            return targetProperty;
        }

        public TMetadata GetMetadata<TMetadata>(PropertyInfo property, EntityDescriptor descriptor, Type descriptorType)
        {
            var metadata = property.GetValue(descriptor) ??
                        throw new EafException($"EntityDescriptor {descriptorType.FullName} initialization failed. Cannot find attribute {property.Name} .");

            return (TMetadata)metadata;
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
