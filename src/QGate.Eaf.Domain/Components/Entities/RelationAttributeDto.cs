namespace QGate.Eaf.Domain.Components.Entities
{
    public class RelationAttributeDto
    {
        public RelationAttributeDto(string attribute, string linkedAttribute)
        {
            Attribute = attribute;
            LinkedAttribute = linkedAttribute;
        }
        public string Attribute { get; set; }
        public string LinkedAttribute { get; set; }
    }
}
