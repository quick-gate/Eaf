using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public abstract class EntityDescriptor: IEntityDescriptor
    {
        
        private static IDictionary<object, RelationDescriptor> _relationDescriptorDictionary = new Dictionary<object, RelationDescriptor>();

        private IDictionary<string, RelationDescriptor> _relationDescriptors = new Dictionary<string, RelationDescriptor>();
        private static readonly IEntityDescriptorFactory _entityDescriptorFactory = Infrastructure.Ioc.ServiceLocator.EntityDescriptorFactory;
        protected EntityDescriptor(Type entityType)
        {
            if(this is IEntityDescriptorContext)
            {
                //todo RESOLVE BETTER
                Entity = new EntityMetadata();
                return;
            }
            _entityDescriptorFactory.Add(this);

            Entity = new EntityMetadata
            {
                Type = entityType
            };
        }

        //protected EntityDescriptor<TRelatedEntity> Get<TRelatedEntity>(Func<EntityDescriptor<TRelatedEntity>> getRelatedEntity, [CallerMemberName]string name = "") where TRelatedEntity: EntityDescriptor<TRelatedEntity>
        //{
        //    if (_relations.TryGetValue(name, out RelationDescriptor relation))
        //    {
        //        return relation as EntityDescriptor<TRelatedEntity>;
        //    }

        //    var relationDescriptor = getRelatedEntity.Invoke() as RelationDescriptor;
        //    var metadata = relationDescriptor.Metadata;
        //    FillMetadataName(metadata, name);
        //    Entity.AddRelation(metadata);
        //    _relations.Add(name, relationDescriptor);

        //    return relationDescriptor as TMetadata;
        //}

        /// <summary>
        /// Has to be called only from descriptor because of caller member name
        /// </summary>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="getMetadata"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual TMetadata Get<TMetadata>(Func<TMetadata> getMetadata, [CallerMemberName]string name = "") where TMetadata : class
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
            else if(getMetadata is Func<RelationDescriptor>)
            {

                if (_relationDescriptors.TryGetValue(name, out RelationDescriptor relation))
                {
                    return relation as TMetadata;
                }

                var relationDescriptor = getMetadata.Invoke() as RelationDescriptor;

                var metadata = relationDescriptor.Metadata;

                FillMetadataName(metadata, name);

                Entity.AddRelation(metadata);

              
                _relationDescriptors.Add(name, relationDescriptor);
                                
                return relationDescriptor as TMetadata;
                //if (Entity.TryGetRelation(name, out RelationMetadata metadata))
                //{
                //    //TODO resolve retun descriptor instead of Metadata
                //    return _relationDescriptorDictionary[metadata] as TMetadata;
                //    return metadata as TMetadata;
                //}

                //var relationDescriptor = getMetadata.Invoke() as RelationDescriptor;
                //metadata = relationDescriptor.Metadata;
                //FillMetadataName(metadata, name);
                //Entity.AddRelation(metadata);

                //_relationDescriptorDictionary.Add(metadata, relationDescriptor);

                //return relationDescriptor as TMetadata;
            }

            //TODO Improve exception
            throw new EafException("Cannot create Attribute");
        }

        private void FillMetadataName(MetadataBase metadata, string name)
        {
            metadata.Name = name;
        }

        public virtual Type EntityType { get; set; }

        public static IEnumerable<EntityDescriptor> GetDescriptors()
        {
            return _entityDescriptorFactory.GetDescriptors();
        }

        public static TEntityDescriptor Get<TEntityDescriptor>() where TEntityDescriptor : EntityDescriptor
        {
            var descriptor = _entityDescriptorFactory.Get<TEntityDescriptor>();
            if (descriptor == null)
            {
                descriptor = Activator.CreateInstance<TEntityDescriptor>();
            }

            return descriptor;
        }

        public static TEntityDescriptor Get<TEntityDescriptor>(IEntityDescriptorContext context) where TEntityDescriptor : EntityDescriptor
        {
            return _entityDescriptorFactory.Get(Get<TEntityDescriptor>(), context);
        }

        public virtual EntityMetadata Entity { get; private set; }

        
    }
}
