namespace QGate.Eaf.Domain.Metadatas.Models
{
    public enum RelationType
    {
        OneToOne,
        /// <summary>
        /// Mapped as OneToMany key is in Referenced entity
        /// </summary>
        OneToOneInverted,
        OneToMany
    }
}
