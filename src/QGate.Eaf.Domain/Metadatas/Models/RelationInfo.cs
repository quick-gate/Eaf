namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class RelationInfo
    {
        public RelationInfo(RelationMetadata relation)
        {
            Relation = relation;
        }

        public RelationMetadata Relation { get; set; }
        public RelationReferenceMetadata RelationReference { get; set; }
    }
}
