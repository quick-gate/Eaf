using System.Collections.Generic;

namespace QGate.Eaf.Domain.Entities.Models.Params
{
    public class GetEntityDetailParams: EntityParamsBase
    {
        public IList<AttributeValue> Keys { get; set; }
    }
}
