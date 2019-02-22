using QGate.Eaf.Domain.Entities.Models.Params;

namespace QGate.Eaf.Domain.Entities.Services
{
    public interface IEntityService
    {
        GetEntityListResult GetEntityList(GetEntityListParams parameters);
    }
}
