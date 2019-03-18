using System.Collections.Generic;

namespace QGate.Eaf.Domain.Entities.Models.Params
{
    public class GetEntityDetailParams: EntityParamsBase
    {
        /// <summary>
        /// Return new instance of entity if null or empty
        /// </summary>
        public IList<AttributeValue> Keys { get; set; }
        public IList<string> IncludePropertyPaths { get; set; }
    }
}
