namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class RelationDescriptorMetadata: RelationMetadata
    {
        private EntityDescriptor _relationDescriptor;
        public new EntityDescriptor EntityReferenceAttribute
        {
            get => _relationDescriptor;
            set
            {
                if(value == null)
                {
                    _relationDescriptor = null;
                    base.EntityReferenceAttribute = null;
                    return;
                }

                var context = (IEntityDescriptorContext)value;
                base.EntityReferenceAttribute = (RelationReferenceMetadata) context.RelationDescriptor.Metadata;

                //base.EntityReferenceAttribute = value.Metadata.EntityReferenceAttribute.Metadata;
            }
        }
    }
}
