namespace QGate.Eaf.Domain.Metadatas.Models
{
    public abstract class RelationDescriptor
    {
        private RelationMetadata _relationMetadata;
        protected IEntityDescriptorContext _context;
        protected EntityDescriptor _entityDescriptor;

        public RelationMetadata Metadata
        {
            get => _relationMetadata;
            set
            {
                _relationMetadata = value;
                if (_relationMetadata != null)
                {
                    _relationMetadata.Entity = _entityDescriptor.Entity;
                }
            }

        }
        public RelationDescriptor()
        {
        }
    }
}
