using System.Collections.Generic;

namespace QGate.Eaf.Domain.Components.Entities
{
    public class EntityList
    {
        public IList<dynamic> Entities { get; set; }

        public IList<EntityListAttribute> Attributes { get; set; }
    }
}
