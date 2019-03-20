namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class AttributeMetadata : AttributeMetadataBase
    {
        public bool IsKey { get; set; }

        public AttributeType AttributeType { get; set; }
    }
}
