using Microsoft.EntityFrameworkCore;
using QGate.Core.Collections;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Linq;
using System.Reflection;

namespace QGate.Eaf.Data.Ef
{
    public class EafDataContext : DbContext
    {
        private readonly IMetadataService _metadataService;
        private static MethodInfo _getSetMethod;
        private static MethodInfo _includeStringMethod;

        public EafDataContext(DbContextOptions options, IMetadataService metadataService) : base(options)
        {
            _metadataService = metadataService;
        }

        public IQueryable Set(Type entityType, params string[] includes)
        {
            if (_getSetMethod == null)
            {
                _getSetMethod = typeof(DbContext).GetMethod("Set");
            }

            var set = (IQueryable)_getSetMethod.MakeGenericMethod(entityType).Invoke(this, null);

            if (includes.IsNullOrEmpty())
            {
                return set;
            }
            if (_includeStringMethod == null)
            {
                _includeStringMethod = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetMethods().Where(x => x.Name == "Include" && x.ToString() == "System.Linq.IQueryable`1[TEntity] Include[TEntity](System.Linq.IQueryable`1[TEntity], System.String)").First();
            }

            foreach (var include in includes)
            {
                set = (IQueryable)_includeStringMethod.MakeGenericMethod(entityType).Invoke(set, new object[] { set, include });
            }

            return set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new MetadataModelBuilder(_metadataService).Build(modelBuilder);
        }
    }
}
