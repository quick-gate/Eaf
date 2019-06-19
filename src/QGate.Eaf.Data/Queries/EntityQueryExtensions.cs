using QGate.Eaf.Domain.Metadatas.Models;

namespace QGate.Eaf.Data.Queries
{
    public static class EntityQueryExtensions
    {
        //public static EntityQueryBuilder<TEntityDescriptorWrapper> Query<TEntityDescriptorWrapper>(this TEntityDescriptorWrapper descriptorWrapper)
        //{
        //    return new EntityQueryBuilder<TEntityDescriptorWrapper>(null);
        //}

        public static EntityQueryBuilder<TDescriptor, TEntity> Query<TDescriptor, TEntity>(this EntityDescriptorWrapper<TDescriptor, TEntity> descriptorWrapper) where TDescriptor : EntityDescriptor<TEntity>
        {
            return new EntityQueryBuilder<TDescriptor, TEntity>(descriptorWrapper);
        }
    }
}
