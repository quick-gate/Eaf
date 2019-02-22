namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class AttributeMetadata: MetadataBase
    {
        public bool IsKey { get; set; }

        public AttributeType AttributeType { get; set; }
    }
}
