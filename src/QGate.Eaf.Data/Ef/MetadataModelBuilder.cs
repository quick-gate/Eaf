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

            entityBuilder.HasKey(entityMetadata.GetKeys().Select(x => x.Name).ToArray());


            foreach (var attribute in entityMetadata.Attributes)
            {
                var propertyBuilder = entityBuilder
                    .Property(attribute.Name)
                    .HasColumnName(attribute.StorageName);

                if(attribute.Length.HasValue)
                {
                    propertyBuilder.HasMaxLength(attribute.Length.Value);
                }

                if(attribute.IsKey)
                {
                    propertyBuilder.ValueGeneratedOnAdd();
                }
                
                if (attribute.Type == null)
                {
                    continue;
                }
               

                if (attribute.AttributeType.Length.HasValue)
                {
                    propertyBuilder.HasMaxLength(attribute.AttributeType.Length.Value);
                }
            }

            foreach (var relation in entityMetadata.Relations)
            {
                if (relation.IsReference)
                {
                    continue;
                }

                //entityBuilder.HasOne(x => x.Description)
                //    .WithOne(x=>x.Product)
                //    .HasForeignKey<ProductDescription>(x => x.ProductId);

                if (relation.RelationType == RelationType.OneToOne || relation.RelationType == RelationType.OneToOneInverted)
                {
                    if (relation.EntityReferenceAttribute != null && relation.EntityReferenceAttribute.RelationType == RelationType.OneToMany)
                    {
                        entityBuilder.HasOne(relation.Entity.Type, relation.Name)
                            .WithMany(relation.EntityReferenceAttribute.Name)
                            .HasForeignKey(relation.Attributes.Select(x=>x.Attribute.Name).ToArray());
                    }
                    else
                    {
                        var referenceBuilder = entityBuilder.HasOne(relation.Entity.Type, relation.Name)
                            .WithOne(relation.EntityReferenceAttribute?.Name)
                            .HasForeignKey(entityMetadata.Type, relation.Attributes.Select(x => x.Attribute.Name).ToArray());
                    }
                }
                else
                {
                    throw new EafException($"Mapping entity {entityMetadata.Name} to model failed. Relation type { relation.RelationType} is not supported.");
                }
            }
        }
    }
}
