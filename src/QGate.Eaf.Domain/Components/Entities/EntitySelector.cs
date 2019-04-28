using QGate.Eaf.Domain.Components.General;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Components.Entities
{
    public class EntitySelector : ComponentBase
    {
        public EntitySelector() : base(ComponentType.EntitySelector)
        {
        }

        public string EntityName { get; set; }
        public IList<string> DisplayAttributes { get; set; }
        /// <summary>
        /// Determines whether related entity is composite of parent entity
        /// </summary>
        public bool IsComposition { get; set; }
        public IList<string> KeyAttributes { get; set; }
        public IList<RelationAttributeDto> RelationAttributes { get; set; }
        public bool IsInverted { get; set; }
    }
}
