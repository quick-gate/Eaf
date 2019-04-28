using QGate.Eaf.Domain.Components.General;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Components.Entities
{
    public class EntityList: ComponentBase
    {
        public EntityList() : base(ComponentType.EntityList)
        {
        }

        public IList<dynamic> Entities { get; set; }

        public IList<EntityListAttribute> Attributes { get; set; }
        public IList<RelationAttributeDto> RelationAttributes { get; set; }
        public string EntityName { get; set; }
    }
}
