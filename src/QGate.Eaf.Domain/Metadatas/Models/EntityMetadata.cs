using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class EntityMetadata : MetadataBase
    {
        public IList<AttributeMetadata> Attributes { get; } = new List<AttributeMetadata>();
        public IList<RelationMetadata> Relations { get; } = new List<RelationMetadata>();
        private IDictionary<string, AttributeMetadata> _attributeDictionary { get; } = new Dictionary<string, AttributeMetadata>();
        private IDictionary<string, RelationMetadata> _relationDictionary { get; } = new Dictionary<string, RelationMetadata>();

        private IList<AttributeMetadata> _keyAttributes { get; } = new List<AttributeMetadata>();

        public void AddAttribute(AttributeMetadata attributeMetadata)
        {
            if (attributeMetadata.IsKey)
            {
                _keyAttributes.Add(attributeMetadata);
            }

            _attributeDictionary.Add(attributeMetadata.Name, attributeMetadata);
            Attributes.Add(attributeMetadata);
        }

        public IList<AttributeMetadata> GetKeys()
        {
            return _keyAttributes;
        }

        public void AddRelation(RelationMetadata relationMetadata)
        {
            _relationDictionary.Add(relationMetadata.Name, relationMetadata);
            Relations.Add(relationMetadata);
        }

        public bool TryGetAttribute(string name, out AttributeMetadata attribute)
        {
            return _attributeDictionary.TryGetValue(name, out attribute);
        }

        public bool TryGetRelation(string name, out RelationMetadata relation)
        {
            return _relationDictionary.TryGetValue(name, out relation);
        }
    }
}
