using QGate.Eaf.Data.Ef;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QGate.Eaf.Data.Queries
{
    public class EntityQueryBuilder<TDescriptorWrapper> // where TDescriptorWrapper : EntityDescriptorWrapper<TDescriptor, TEntity>
    {
        //public IList<TEntity> ToList()
        //{

        //}
        public EntityQueryBuilder(EafDataContext dataContext) //: base(dataContext)
        {
        }

        public void Select(Expression<Func<TDescriptorWrapper, object>> selector)
        {
            var path = selector.Body.ToString();
        }
        //public IList<TEntity> FirstOrDefault()
        //{
        //    return null;
        //}
    }
}
