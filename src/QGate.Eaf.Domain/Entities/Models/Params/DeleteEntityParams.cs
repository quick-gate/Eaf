using System.Collections.Generic;

namespace QGate.Eaf.Domain.Entities.Models.Params
{
    public class DeleteEntityParams: EntityParamsBase
    {
        public IList<AttributeValue> Keys { get; set; }
    }
}
