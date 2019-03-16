namespace QGate.Eaf.Domain.Entities.Models.Params
{
    public class SaveEntityParams: EntityParamsBase
    {
        public dynamic Entity { get; set; }
        public bool IsNew { get; set; }
    }
}
