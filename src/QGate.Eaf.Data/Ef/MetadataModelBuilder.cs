using Microsoft.EntityFrameworkCore;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Services;
using System.Linq;

namespace QGate.Eaf.Data.Ef
{
    public class MetadataModelBuilder
    {
        private readonly IMetadataService _metadataService;
        public MetadataModelBuilder(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public void Build(ModelBuilder modelBuilder)
        {
            foreach (var entityMetadata in _metadataService.GetEntityMetadatas())
            {
                BuildEntity(modelBuilder, entityMetadata);
            }
        }

        private void BuildEntity(ModelBuilder modelBuilder, EntityMetadata entityMetadata)
        {
            var entityBuilder = modelBuilder.Entity(entityMetadata.Type);
            var tableAndSchema = entityMetadata.StorageName.Split('.');
            if(tableAndSchema.Length > 1)
            {
                entityBuilder.ToTable(tableAndSchema[1], tableAndSchema[0]);
            }
            else
            {
                entityBuilder.ToTable(entityMetadata.StorageName);
            }

            entityBuilder.HasKey(entityMetadata.KeyAttributes.Select(x => x.Name).ToArray());

            foreach (var attribute in entityMetadata.Attributes.Values)
            {
                var propertyBuilder = entityBuilder
                    .Property(attribute.Name)
                    .HasColumnName(attribute.StorageName);
                
                if (attribute.Type == null)
                {
                    continue;
                }

                if (attribute.AttributeType.Length.HasValue)
                {
                    propertyBuilder.HasMaxLength(attribute.AttributeType.Length.Value);
                }
            }

            foreach (var relation in entityMetadata.Relations.Values)
            {
                //entityBuilder.HasOne(x => x.Description)
                //    .WithOne(x=>x.Product)
                //    .HasForeignKey<ProductDescription>(x => x.ProductId);

                if (relation.RelationType == RelationType.OneToOne)
                {
                    entityBuilder.HasOne(relation.Entity.Type, relation.Name)
                        .WithOne(relation.AttributeName)
                        .HasForeignKey(entityMetadata.Type, relation.Keys.Select(x => x.Name).ToArray());
                }
                else if (relation.RelationType == RelationType.OneToMany)
                {
                    entityBuilder.HasOne(relation.Entity.Type, relation.Name)
                        .WithMany(relation.AttributeName)
                        .HasForeignKey(relation.Keys.Select(x => x.Name).ToArray());
                }
                else
                {
                    throw new EafException($"Mapping entity {entityMetadata.Name} to model failed. Relation type { relation.RelationType} is not supported.");
                }
            }

            
        }
    }
}
