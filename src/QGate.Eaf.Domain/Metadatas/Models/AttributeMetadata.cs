namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class AttributeMetadata : AttributeMetadataBase
    {
        public bool IsKey { get; set; }
        /// <summary>
        /// Determines whether is this property used as relation Key.
        /// </summary>
        public bool IsRelationKey { get; set; }

        public AttributeType AttributeType { get; set; }
        /// <summary>
        /// Determines max length of string values
        /// </summary>
        public int? Length { get; set; }

    }
}
