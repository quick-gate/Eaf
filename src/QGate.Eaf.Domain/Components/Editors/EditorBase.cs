using QGate.Eaf.Domain.Components.General;

namespace QGate.Eaf.Domain.Components.Editors
{
    public abstract class EditorBase : ComponentBase
    {
        public EditorBase(ComponentType type) : base(type)
        {
        }

        public bool IsReadOnly { get; set; }
    }
}
