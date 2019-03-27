namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class AttributeMetadataBase: MetadataBase
    {
        /// <summary>
        /// Owner of attribute or relation
        /// </summary>
        public EntityMetadata Owner { get; set; }

        public string Path { get; set; }
    }
}
