using Microsoft.AspNetCore.Mvc;
using QGate.Eaf.Domain.Entities.Models.Params;
using QGate.Eaf.Domain.Entities.Services;

namespace QGate.Eaf.AspNetCore.Controllers
{
    [Route(BaseUrl + "entities")]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [Route("get-list")]
        [HttpPost]
        public GetEntityListResult GetEntityList([FromBody]GetEntityListParams parameters)
        {
            var result = _entityService.GetEntityList(parameters);
            return result;
        }

        [Route("get-detail")]
        [HttpPost]
        public GetEntityDetailResult GetEntityDetail([FromBody]GetEntityDetailParams parameters)
        {
            return _entityService.GetEntityDetail(parameters);
        }

        [Route("")]
        [HttpPost]
        public SaveEntityResult SaveEntity([FromBody] SaveEntityParams parameters)
        {
            return _entityService.SaveEntity(parameters);
        }

        [Route("delete")]
        [HttpPost]
        public DeleteEntityResult DeleteEntity([FromBody] DeleteEntityParams parameters)
        {
            return _entityService.DeleteEntity(parameters);
        }
    }
}
