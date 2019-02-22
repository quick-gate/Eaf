using QGate.Eaf.Domain.Entities.Models.Params;
using System.Collections.Generic;

namespace QGate.Eaf.Core.Data
{
    public interface EntityRepository
    {
        IList<dynamic> GetEntityList(GetEntityListParams parameters);
    }
}
