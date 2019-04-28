namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class RelationAttribute
    {
        public RelationAttribute(AttributeMetadata attribute, AttributeMetadata linkedAttribute)
        {
            Attribute = attribute;
            LinkedAttribute = linkedAttribute;


            SetIsRelationKey(attribute);
            SetIsRelationKey(linkedAttribute);
        }
        public AttributeMetadata Attribute { get; set; }
        public AttributeMetadata LinkedAttribute { get; set; }

        private void SetIsRelationKey(AttributeMetadata attribute)
        {
            if (!attribute.IsKey)
            {
                attribute.IsRelationKey = true;
            }
        }
    }
}
