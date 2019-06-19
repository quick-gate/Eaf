using QGate.Eaf.Data.Ef;
using QGate.Eaf.Domain.Metadatas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace QGate.Eaf.Data.Queries
{
    public class EntityQueryBuilder
    {
        private EafDataContext _dataContext;
        private ISet<string> _selectPaths = new HashSet<string>();
        protected IDbConnection _connection;

        public EntityQueryBuilder(EafDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public EntityQueryBuilder()
        {

        }

        

    }
}
