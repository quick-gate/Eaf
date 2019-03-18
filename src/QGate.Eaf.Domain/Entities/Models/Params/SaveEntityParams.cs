namespace QGate.Eaf.Domain.Entities.Models.Params
{
    public class SaveEntityParams: EntityParamsBase
    {
        public dynamic Entity { get; set; }
        public bool IsNew { get; set; }
        /// <summary>
        /// Determines whether is required return refreshed entity list item after entity save
        /// </summary>
        public bool IsFillEntityListItemRequired { get; set; }
    }
}
