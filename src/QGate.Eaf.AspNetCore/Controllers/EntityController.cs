using Microsoft.AspNetCore.Mvc;
using QGate.Eaf.Domain.Entities.Models.Params;
using QGate.Eaf.Domain.Entities.Services;

namespace QGate.Eaf.AspNetCore.Controllers
{
    [Route("api/eaf/entities")]
    public class EntityController : Controller
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
            return _entityService.GetEntityList(parameters);
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetEntityList()
        {
            return Ok("olee");
        }
    }
}
