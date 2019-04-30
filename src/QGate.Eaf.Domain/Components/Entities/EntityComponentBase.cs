using QGate.Eaf.Domain.Components.General;

namespace QGate.Eaf.Domain.Components.Entities
{
    public class EntityComponentBase : ComponentBase
    {
        public string EntityName { get; set; }
        public string EntityCaption { get; set; }
        public EntityComponentBase(ComponentType type) : base(type)
        {
        }
    }
}
