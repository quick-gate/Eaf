namespace QGate.Eaf.Domain.Entities.Models
{
    public class AttributeValue
    {
        public AttributeValue()
        {

        }

        public AttributeValue(string name, object value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Attribute Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Attribute Value
        /// </summary>
        public object Value { get; set; }
    }
}
