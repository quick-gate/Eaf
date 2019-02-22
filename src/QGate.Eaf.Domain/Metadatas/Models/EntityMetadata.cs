using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class EntityMetadata : MetadataBase
    {
        public IDictionary<string, AttributeMetadata> Attributes { get; } = new Dictionary<string, AttributeMetadata>();
        public IDictionary<string, RelationMetadata> Relations { get; } = new Dictionary<string, RelationMetadata>();

        public IList<AttributeMetadata> KeyAttributes { get; } = new List<AttributeMetadata>();

        public void AddAttribute(AttributeMetadata attributeMetadata)
        {
            if (attributeMetadata.IsKey)
            {
                KeyAttributes.Add(attributeMetadata);
            }

            Attributes.Add(attributeMetadata.Name, attributeMetadata);
        }

        public void AddRelation(RelationMetadata relationMetadata)
        {
            Relations.Add(relationMetadata.Name, relationMetadata);
        }
    }
}
