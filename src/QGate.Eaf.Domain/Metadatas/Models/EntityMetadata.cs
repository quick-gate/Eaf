using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class EntityMetadata : MetadataBase
    {
        private IDictionary<string, MetadataBase> _memberDictionary = new Dictionary<string, MetadataBase>();
        public IList<AttributeMetadata> Attributes { get; } = new List<AttributeMetadata>();
        public IList<RelationMetadata> Relations { get; } = new List<RelationMetadata>();
        public IList<RelationInfo> RelationInfos { get; } = new List<RelationInfo>();
        private IDictionary<string, AttributeMetadata> _attributeDictionary { get; } = new Dictionary<string, AttributeMetadata>();
        private IDictionary<string, RelationMetadata> _relationDictionary { get; } = new Dictionary<string, RelationMetadata>();

        private IList<AttributeMetadata> _keyAttributes { get; } = new List<AttributeMetadata>();

        public void AddAttribute(AttributeMetadata attributeMetadata)
        {
            FillAttributeAndRelationBase(attributeMetadata);

            if (attributeMetadata.IsKey)
            {
                _keyAttributes.Add(attributeMetadata);
            }

            _attributeDictionary.Add(attributeMetadata.Name, attributeMetadata);
            AddMemberInternal(attributeMetadata);
            Attributes.Add(attributeMetadata);
        }

        public IList<AttributeMetadata> GetKeys()
        {
            return _keyAttributes;
        }

        private void FillAttributeAndRelationBase(AttributeMetadataBase metadataBase)
        {
            metadataBase.Owner = this;
        }

        public void AddRelation(RelationMetadata relationMetadata)
        {
            FillAttributeAndRelationBase(relationMetadata);
            _relationDictionary.Add(relationMetadata.Name, relationMetadata);
            AddMemberInternal(relationMetadata);
            Relations.Add(relationMetadata);
        }

        private void AddMemberInternal(AttributeMetadataBase member)
        {
            _memberDictionary.Add(member.Name, member);
        }

        public bool TryGetAttribute(string name, out AttributeMetadata attribute)
        {
            return _attributeDictionary.TryGetValue(name, out attribute);
        }

        public bool TryGetRelation(string name, out RelationMetadata relation)
        {
            return _relationDictionary.TryGetValue(name, out relation);
        }

        public bool TryGetMember(string name, out MetadataBase member)
        {
            return _memberDictionary.TryGetValue(name, out member);
        }
    }
}
