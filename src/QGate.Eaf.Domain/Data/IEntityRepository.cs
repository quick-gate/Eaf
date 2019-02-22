using QGate.Eaf.Domain.Entities.Models.Params;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Data
{
    public interface IEntityRepository
    {
        IList<dynamic> GetEntityList(GetEntityListParams parameters);
    }
}
