using FastMember;
using QGate.Core.Exceptions;
using QGate.Eaf.Data.Ef;
using QGate.Eaf.Data.Queries.Internals;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;
using Slapper;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace QGate.Eaf.Data.Queries
{
    public class EntityQueryBuilder<TDescriptor, TEntity>: EntityQueryBuilder  where TDescriptor : EntityDescriptor<TEntity>
    {
        EntityDescriptorWrapper<TDescriptor, TEntity> _descriptorWrapper;
        private EntityMetadata _entity;
        private Query _query;
        private QueryFactory _queryFactory = new QueryFactory();
        public EntityQueryBuilder(EntityDescriptorWrapper<TDescriptor, TEntity> descriptorWrapper)
        {
            _descriptorWrapper = descriptorWrapper;
            _entity = _descriptorWrapper.Entity;

            _query = new Query(_entity.StorageName + " as t0");

            //var compiledQuery = new SqlKata.Compilers.SqlServerCompiler().Compile(query);
            
            
        }

        

        public EntityQueryBuilder(EafDataContext dataContext) : base(dataContext)
        {
        }

        public IList<TEntity> ToList()
        {
            return null;
        }

        public IList<TResult> ToList<TResult>()
        {
            return null;
        }

        public EntityQueryBuilder<TDescriptor, TEntity> AddConnection(IDbConnection connection)
        {
            _connection = connection;
            return this;
        }

        public EntityQueryBuilder<TDescriptor, TEntity> Select(params Expression<Func<TDescriptor, object>>[] selectors)
        {
            Throw.IfNullOrEmpty(selectors, nameof(selectors));

            foreach (var selector in selectors)
            {
                SelectInternal(selector);
            }

            return this;
        }

        private void SelectInternal(Expression<Func<TDescriptor, object>> selector)
        {
            var propertyPath = selector.Body.ToString();
            var indexOfFirstDot = propertyPath.IndexOf('.');
            if(indexOfFirstDot > -1)
            {
                propertyPath = propertyPath.Substring(indexOfFirstDot + 1);
            }

            var properties = propertyPath.Split('.');


            var currentEntity = _entity;
            var tableIndex = 0;
            var currentTableAlias = GetTableAlias(tableIndex);

            IList<QColumn> columns = new List<QColumn>();

            var tableAliasDictionary = new Dictionary<string, string>
            {
                [currentEntity.StorageName] = currentTableAlias
            };

            foreach (var property in properties)
            {
                if (!currentEntity.TryGetMember(property, out MetadataBase member))
                {
                    throw new EafException($"Cannot find member {property}");
                }

                if(member is AttributeMetadata)
                {
                    var attribute = (AttributeMetadata)member;
                    var column = new QColumn
                    {
                        Properties = properties,
                        Path = propertyPath,
                        Alias = propertyPath.Replace('.', '_')
                    };

                    columns.Add(column);
                    _query.Select(GetColumnName(currentTableAlias, member.Name, column.Alias));
                }
                else
                {

                    var relatedTableAlias = GetTableAlias(++tableIndex);
                    var relation = (RelationMetadata)member;
                    currentEntity = relation.Entity;
                    _query.CombineRaw($"LEFT JOIN {relation.Entity.StorageName} {relatedTableAlias} ON {currentTableAlias}.{relation.Attributes[0].Attribute.StorageName} = {relatedTableAlias}.{relation.Attributes[0].LinkedAttribute.StorageName}");

                    currentTableAlias = relatedTableAlias;
                }

               
                

                
            }


            var compiledQuery = new SqlKata.Compilers.SqlServerCompiler().Compile(_query);


            var accessor = TypeAccessor.Create(_entity.Type);


            using (var connection = _connection)
            {
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = compiledQuery.RawSql;
                    using (var reader = command.ExecuteReader())
                    {
                        
                        while(reader.Read())
                        {
                            //var entity = 

                            //var cellDictionary = new Dictionary<string, object>();
                            var entity = Activator.CreateInstance<TEntity>();
                            foreach (var column in columns)
                            {
                                if(column.Properties.Count > 0)
                                {
                                    foreach (var property in column.Properties)
                                    {

                                    }
                                }

                                //cellDictionary.Add(column.Alias, reader[column.Alias]);
                            }


                        }
                    }
                }
            }

        }

        private string GetColumnName(string tableName, string columnName, string columnAlias)
        {
            return string.Concat(tableName, ".", columnName, string.IsNullOrWhiteSpace(columnAlias)? null: string.Concat(" AS ", columnAlias));
        }

        private string GetTableAlias(int index)
        {
            return string.Concat("t", index);
        }

        //public IList<TEntity> FirstOrDefault()
        //{
        //    return null;
        //}
    }
}
