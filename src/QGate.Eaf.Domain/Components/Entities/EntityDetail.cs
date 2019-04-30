using QGate.Eaf.Domain.Components.General;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Components.Entities
{
    public class EntityDetail: EntityComponentBase
    {
        public EntityDetail() : base(ComponentType.EntityDetail)
        {
        }

        public dynamic Entity { get; set; }
        public IList<ComponentBase> Components { get; set; }
    }
}
