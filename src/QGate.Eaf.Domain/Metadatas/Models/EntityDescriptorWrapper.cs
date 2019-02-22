namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class EntityDescriptorWrapper<TDescriptor, TEntity> : EntityDescriptor<TEntity> where TDescriptor : EntityDescriptor<TEntity>
    {
        public static TDescriptor Instance => Get<TDescriptor>();
    }
}
