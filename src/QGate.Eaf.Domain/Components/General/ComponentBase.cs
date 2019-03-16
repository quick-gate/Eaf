namespace QGate.Eaf.Domain.Components.General
{
    public abstract class ComponentBase
    {
        public ComponentBase(ComponentType type)
        {
            Type = type;
        }
        public ComponentType Type { get; private set; }

        public ComponentBinding Binding { get; set; }
    }
}
