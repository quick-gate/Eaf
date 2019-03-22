namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class RelationDescriptor<TRelatedEntityDescriptor>: RelationDescriptor where TRelatedEntityDescriptor: EntityDescriptor
    {
        public RelationDescriptor(): base()
        {
            //Creating Proxy for related entity
            _entityDescriptor = EntityDescriptor.Get<TRelatedEntityDescriptor>(new EntityDescriptorContext
            {
                RelationDescriptor = this
            });
        }

        public TRelatedEntityDescriptor Entity
        {
            get
            {
                return (TRelatedEntityDescriptor) _entityDescriptor;
            }
        }
    }
}
