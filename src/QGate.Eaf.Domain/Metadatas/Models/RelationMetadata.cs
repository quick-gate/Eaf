﻿using QGate.Eaf.Domain.Metadatas.Models;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class RelationMetadata: MetadataBase
    {
        /// <summary>
        /// Relation Key from current Entity
        /// </summary>
        public IList<AttributeMetadata> Keys { get; set; }
        /// <summary>
        /// Related navigation property name
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// Related Entity
        /// </summary>
        public EntityMetadata Entity { get; set; }
        /// <summary>
        /// Relation Type
        /// </summary>
        public RelationType RelationType { get; set; }
    }
}
