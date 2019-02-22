namespace QGate.Eaf.Domain.Metadatas.Models
{
    public abstract class EntityDescriptor<TEntity>: EntityDescriptor
    {
        public EntityDescriptor() : base(typeof(TEntity))
        {
        }
    }
}
