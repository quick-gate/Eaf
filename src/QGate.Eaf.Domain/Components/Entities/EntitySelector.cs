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
        public IList<string> KeyAttributes { get; set; }
        public IList<string> RelationAttributes { get; set; }
    }
}
