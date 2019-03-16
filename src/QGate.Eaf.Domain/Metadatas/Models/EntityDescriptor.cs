using QGate.Eaf.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public abstract class EntityDescriptor: IEntityDescriptor
    {
        private static IDictionary<Type, EntityDescriptor> _entityDescriptors = new Dictionary<Type, EntityDescriptor>();

        public EntityDescriptor(Type entityType)
        {
            var descriptorType = GetType();
            if (!_entityDescriptors.ContainsKey(descriptorType))
            {
                _entityDescriptors.Add(descriptorType, this);
                Entity = new EntityMetadata
                {
                    Type = entityType
                };
            }
        }

        /// <summary>
        /// Has to be called only from descriptor because of caller member name
        /// </summary>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="getMetadata"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected TMetadata Get<TMetadata>(Func<TMetadata> getMetadata, [CallerMemberName]string name = "") where TMetadata : class
        {
            if (getMetadata is Func<AttributeMetadata>)
            {
                if (Entity.TryGetAttribute(name, out AttributeMetadata metadata))
                {
                    return metadata as TMetadata;
                }

                metadata = getMetadata.Invoke() as AttributeMetadata;
                FillMetadataName(metadata, name);
                Entity.AddAttribute(metadata);
                return metadata as TMetadata;

            }
            else if (getMetadata is Func<RelationMetadata>)
            {
                if (Entity.TryGetRelation(name, out RelationMetadata metadata))
                {
                    return metadata as TMetadata;
                }

                metadata = getMetadata.Invoke() as RelationMetadata;
                FillMetadataName(metadata, name);
                Entity.AddRelation(metadata);
                return metadata as TMetadata;
            }

            //TODO Improve exception
            throw new EafException("Cannot create Attribute");
        }

        private void FillMetadataName(MetadataBase metadata, string name)
        {
            metadata.Name = name;
        }

        public Type EntityType { get; set; }

        public static IEnumerable<EntityDescriptor> GetDescriptors()
        {
            return _entityDescriptors.Values;
        }

        public static TEntityDescriptor Get<TEntityDescriptor>() where TEntityDescriptor : EntityDescriptor
        {
            var descriptorType = typeof(TEntityDescriptor);
            _entityDescriptors.TryGetValue(descriptorType, out EntityDescriptor descriptor);
            if (descriptor == null)
            {
                descriptor = Activator.CreateInstance<TEntityDescriptor>();
            }

            return descriptor as TEntityDescriptor;
        }

        public EntityMetadata Entity { get; private set; }
    }
}
