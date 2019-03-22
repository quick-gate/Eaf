using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class RelationMetadata: AttributeMetadataBase
    {
        /// <summary>
        /// Related Entity
        /// </summary>
        public EntityMetadata Entity { get; set; }
        /// <summary>
        /// Reference attribute (Navigation property) from related entity
        /// </summary>
        public RelationReferenceMetadata EntityReferenceAttribute { get; set; }
        /// <summary>
        /// Keys from current Entity
        /// </summary>
        public IList<AttributeMetadata> Keys { get; set; }
        /// <summary>
        /// Relation Type
        /// </summary>
        public RelationType RelationType { get; set; }
        /// <summary>
        /// Determines whether is Reference (Navigation property) attribute.
        /// </summary>
        public bool IsReference { get; set; }

        //public RelationMetadata SetEntityReference(EntityDescriptor entityDescriptor, [CallerMemberName] string attributeName="")
        //{
            
        //    return this;
        //}

    }
}
