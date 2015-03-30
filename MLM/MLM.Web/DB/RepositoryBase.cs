using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Configuration;
using MLM.Models;
using MLM.Logging;
using MLM.Exception;
using MLM.DB.Specifications;
using MLM.DB.PagedList;


namespace MLM.DB
{
    /// <summary>
    /// Provides basic implementation of a Repository.
    /// The default implementation is to fetch a entity from a database table.
    /// </summary>
    /// <typeparam name="T">A EF mapped database Entity</typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : MLMBaseEntity
    {
        private DbContext database = null;
        private readonly IDbSet<T> dbSet;
        public ILog Logger { get; set; }
        public IExceptionHandler ExceptionHandler { get; set; }

        public RepositoryBase()
        {
            Logger = new Logger(this.GetType());
            database = new MLMDbContext("MLMCon");
            dbSet = database.Set<T>();
        }

        public virtual T Add(T entity)
        {
            var addedEntity = dbSet.Add(entity);
           
            return addedEntity;
        }

        public virtual T Update(T entity)
        {
            var tracker = database.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity.Id == entity.Id);

            //If we have entity already in tracked context then return that tracked entity, else attach the new entity and return that entity;
            if (tracker != null && tracker.Entity != null)
            {
                database.Entry(tracker.Entity).State = EntityState.Detached;
                //return tracker.Entity;
            }

            T updated = dbSet.Attach(entity);
            database.Entry(entity).State = EntityState.Modified;

          
            return updated;
        }

        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }

        public virtual IEnumerable<T> Delete(ISpecification<T> specification)
        {
            var query = dbSet.Where(specification.Predicate);
            foreach (T obj in query)
                dbSet.Remove(obj);
            return null;
        }

        public virtual T GetById(long id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual T Get(ISpecification<T> specification)
        {
            return GetMany(specification).FirstOrDefault();
        }

        public TDerived Get<TDerived>(ISpecification<TDerived> specification) where TDerived : class, T
        {
            return GetMany(specification).FirstOrDefault();
        }
        //Generic method  to implemenent for entity framework
        public virtual IEnumerable<T> GetMany(long userId, long courseId)
        {
            throw new NotImplementedException();
        }
        public virtual IQueryable<T> GetAll()
        {
            //TODO: Add security here to ensure records only in the currents "users context are returned"
            return database.Set<T>();
        }

        public virtual IEnumerable<T> GetMany(ISpecification<T> specification)
        {
            var query = GetManyInternal(specification);
            //query = ApplyFetchProjections(specification, query);
            var result = query;
            SelectFetchProjections(specification, result);
            return result.ToList(); //TODO: Always return paged result with some hardcoded value

        }

        protected virtual IQueryable<T> GetManyInternal(ISpecification<T> specification)
        {
            var FilteredSet = dbSet.Where(specification.Predicate);
            //IQueryable<T> query = objectContext.CreateObjectSet<T>();
            //query = ApplyFetchProjections(specification, query);
            //query = query.Where(specification.Predicate);
            //TODO: Exclude IsDeleted only when the model supports it.
            //query = ExcludeIsDeleted(query);
            //ObjectQuery objQuery = query as ObjectQuery;
            //String str = objQuery.ToTraceString();
            return FilteredSet;
        }

        protected virtual IQueryable<T> ExcludeIsDeleted(IQueryable<T> query)
        {
            return query.Where(t => !t.IsDeleted);
        }

        public virtual IPagedList<T> GetMany(ISpecification<T> specification, IPagingStrategy<T> pagingStrategy)
        {
            var query = GetManyInternal(specification);
            //query = GetQueryWithPaging(pagingStrategy, query);
            ////query = ApplyFetchProjections(specification, query);          
            //var result = query.ToPagedList<T>(pagingStrategy.PageNumber, pagingStrategy.PageSize);
            var result = ApplyQueryPaging(pagingStrategy, query);
            SelectFetchProjections(specification, result);
            return result;
        }

        public virtual IEnumerable<T> GetMany(ISpecification<T> specification, params Expression<Func<T, object>>[] fetchStrategies)
        {
            foreach (var propertyLambda in fetchStrategies)
            {
                specification.FetchStrategy.Include(propertyLambda);
            }
            return GetMany(specification);
        }

        public virtual IEnumerable<TDerived> GetMany<TDerived>(ISpecification<TDerived> specification) where TDerived : class, T
        {
            var query = dbSet.OfType<TDerived>().Where(specification.Predicate);
            return query;
        }

        public virtual bool Matches(ISpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        private void SelectFetchProjections(ISpecification<T> spec, IEnumerable<T> result)
        {
            if (spec.FetchStrategy == null)
            {
                return;
            }

            foreach (var path in spec.FetchStrategy.IncludePaths)
            {
                //if (!(path.Body is MemberExpression))
                //    throw new NotSupportedException("The include paths only support member access lambda expressions");

                //MemberExpression selector = path.Body as MemberExpression;
                //if (selector.Expression.NodeType != ExpressionType.TypeAs)
                //    continue;   //skip expressions that are not TypeAs

                //IQueryable<T> includePathProjection = objectContext.CreateObjectSet<T>();
                IEnumerable<long> ids = result.Select(t => t.Id);
                var FilteredSet = dbSet.Where(p => ids.Contains(p.Id));
                FilteredSet.Select(path).ToList(); //TODO: This selects the path and does the "fix-up". Review if this is okay.
            }
        }

        private void SelectFetchProjections(ISpecification<T> spec, IQueryable<T> result)
        {
            if (spec.FetchStrategy == null)
            {
                return;
            }

            foreach (var path in spec.FetchStrategy.IncludePaths)
            {
                //if (!(path.Body is MemberExpression))
                //    throw new NotSupportedException("The include paths only support member access lambda expressions");

                //MemberExpression selector = path.Body as MemberExpression;
                //if (selector.Expression.NodeType != ExpressionType.TypeAs)
                //    continue;   //skip expressions that are not TypeAs

                //IQueryable<T> includePathProjection = objectContext.CreateObjectSet<T>();
                IEnumerable<long> ids = result.Select(t => t.Id);
                var FilteredSet = dbSet.Where(p => ids.Contains(p.Id));
                FilteredSet.Select(path).ToList(); //TODO: This selects the path and does the "fix-up". Review if this is okay.
            }
        }

        private IQueryable<T> ApplyFetchProjections(ISpecification<T> spec, IQueryable<T> query)
        {
            if (spec.FetchStrategy == null)
            {
                return query;
            }

            foreach (var path in spec.FetchStrategy.IncludePaths)
            {
                // TypeAs expression (for e.g. (l => l As AssignmentLineItem).AssignmentAdministration) is not supported in the ObjectQuery.Include
                // See https://connect.microsoft.com/VisualStudio/feedback/details/594289/in-entity-framework-there-should-be-a-way-to-eager-load-include-navigation-properties-of-a-derived-class to track support for it
                // As a workaround, those include paths would be skipped in the include. 
                // They would be projected separately after the underlying entity set is queried (See this.ApplyFetchProjection)

                if (!(path.Body is MemberExpression))
                    throw new NotSupportedException("The include paths only support member access lambda expressions");

                MemberExpression selector = path.Body as MemberExpression;
                if (selector.Expression.NodeType == ExpressionType.TypeAs)
                    continue;   //skip TypeAs expression

                query = query.Include(path);
            }
            return query;
        }

        private IQueryable<U> GetQueryWithPaging<U>(IPagingStrategy<U> pagingStrategy, IQueryable<U> query) where U : class
        {
            if (pagingStrategy.OrderStrategy == null)
                return query;

            if (!pagingStrategy.OrderStrategy.OrderingKeyProperties.Any())
                return query;

            IOrderedQueryable<U> orderedQuery = null;
            foreach (var orderingKeyProperty in pagingStrategy.OrderStrategy.OrderingKeyProperties)
            {
                switch (orderingKeyProperty.Value)
                {
                    case SortOrder.NONE:
                    case SortOrder.ASC:
                        if (orderedQuery == null)
                        {
                            orderedQuery = query.OrderByProperty(orderingKeyProperty.Key);
                            continue;
                        }
                        orderedQuery = orderedQuery.ThenByProperty(orderingKeyProperty.Key);
                        break;
                    case SortOrder.DESC:
                        if (orderedQuery == null)
                        {
                            orderedQuery = query.OrderByDescendingProperty(orderingKeyProperty.Key);
                            continue;
                        }
                        orderedQuery = orderedQuery.ThenByDescendingProperty(orderingKeyProperty.Key);
                        break;
                }
            }
            return orderedQuery;
        }

        private IPagedList<T> ApplyQueryPaging(IPagingStrategy<T> pagingStrategy, IQueryable<T> query)
        {
            IEnumerable<T> result = null;
            query = GetQueryWithPaging(pagingStrategy, query);
            //TODO: Review this entire if block. Only first and last records are supported. Make it generic.
            if (pagingStrategy.PagingSpecifications != null && pagingStrategy.PagingSpecifications.Any())
            {
                //var tempQuery = objectContext.CreateObjectSet<T>();
                int skipCount = 0;
                IPagingSpecification<T> topPaginSpec = pagingStrategy.PagingSpecifications.Where(ps => ps.PagingPosition == PagingPosition.Top).FirstOrDefault();
                IPagingSpecification<T> bottomPaginSpec = pagingStrategy.PagingSpecifications.Where(ps => ps.PagingPosition == PagingPosition.Bottom).FirstOrDefault();
                IList<T> firstRecord = dbSet.Where(topPaginSpec.PagingSpecificationAtPosition.Predicate).ToList();
                IList<T> lastRecord = dbSet.Where(bottomPaginSpec.PagingSpecificationAtPosition.Predicate).ToList();
                long itemCount = query.LongCount() + firstRecord.Count + lastRecord.Count;
                //foreach (IPagingSpecification<T> pagingSpecification in pagingStrategy.PagingSpecifications)
                //{

                //long position = pagingSpecification.PostionEvaluator(pagingStrategy, itemCount);
                //if (pagingStrategy.IsInCurrentPage(position) && position == 1)
                //{
                //    firstRecord = tempQuery.Where(pagingSpecification.PagingSpecificationAtPosition.Predicate).ToList();
                //    if (firstRecord.Count() > 0)
                //        skipCount++;

                //}
                //if (pagingStrategy.IsInCurrentPage(position) && position == itemCount)
                //{
                //    lastRecord = tempQuery.Where(pagingSpecification.PagingSpecificationAtPosition.Predicate).ToList();
                //    if (lastRecord.Count() > 0)
                //        skipCount++;
                //}
                //}
                firstRecord = pagingStrategy.IsInCurrentPage(1) ? firstRecord ?? new List<T>() : new List<T>();
                lastRecord = pagingStrategy.IsInCurrentPage(itemCount) ? lastRecord ?? new List<T>() : new List<T>();
                result = query.Skip((pagingStrategy.PageNumber - 1) * pagingStrategy.PageSize).Take(pagingStrategy.PageSize - (firstRecord.Count + lastRecord.Count)).ToList();
                //result = result.Where(t => !(firstRecord.Union(lastRecord)).Select(x => x.Id).Contains((t.Id)));
                result = (firstRecord).Union(result).Union(lastRecord);
                return result.ToPagedList(pagingStrategy.PageNumber, pagingStrategy.PageSize, (int)itemCount + skipCount); //TODO: Assumption here that total count where never be long
            }
            return query.ToPagedList<T>(pagingStrategy.PageNumber, pagingStrategy.PageSize);
        }

        public long Count(ISpecification<T> spec)
        {
            var query = GetManyInternal(spec);
            return query.LongCount();
        }

        public IEnumerable<long> GetIds(ISpecification<T> spec)
        {
            var query = GetManyInternal(spec);
            return query.Select(t => t.Id).ToList();
        }

        public IEnumerable<long> GetIds<TDerived>(ISpecification<TDerived> specification) where TDerived : class,T
        {
            var query = dbSet.OfType<TDerived>().Where(specification.Predicate);
            return query.Select(t => t.Id).ToList();
        }


        public virtual IEnumerable<T> Add(IEnumerable<T> entity)
        {
            //throw new NotImplementedException();
            return null;
        }


        public IEnumerable<T> GetMany(IEnumerable<long> ids)
        {
            return dbSet.Where(t => ids.Contains(t.Id)).ToList();
        }


        public IQueryable<T> GetAll(ISpecification<T> specification)
        {
            var query = GetManyInternal(specification);

            var result = query;
            SelectFetchProjections(specification, result);
            return result;
        }

        public IEnumerable<T> ExecuteStoreQuery<T>(string sql, params object[] parameters)
        {
            return ((IObjectContextAdapter)database).ObjectContext.ExecuteStoreQuery<T>(sql, parameters);
        }
    }



    public static class ReflectionQueryable
    {
        private static readonly MethodInfo OrderByMethod = GetOrderByMethodInfo(typeof(Queryable), "OrderBy");
        private static readonly MethodInfo ThenByMethod = GetOrderByMethodInfo(typeof(Queryable), "ThenBy");
        private static readonly MethodInfo OrderByDescendingMethod = GetOrderByMethodInfo(typeof(Queryable), "OrderByDescending");
        private static readonly MethodInfo ThenByDescendingMethod = GetOrderByMethodInfo(typeof(Queryable), "ThenByDescending");

        private static MethodInfo GetOrderByMethodInfo(Type type, string methodName)
        {
            return type.GetMethods()
                .Where(method => method.Name == methodName)
                .Where(method => method.GetParameters().Length == 2)
                .Single();
        }

        public static IOrderedQueryable<TSource> OrderByProperty<TSource>
            (this IQueryable<TSource> source, Expression<Func<TSource, object>> keySelector)
        {
            var IQueryableMethod = OrderByMethod;
            return CallIQueryableMethod<TSource>(source, keySelector, IQueryableMethod);
        }

        public static IOrderedQueryable<TSource> ThenByProperty<TSource>
            (this IOrderedQueryable<TSource> source, Expression<Func<TSource, object>> keySelector)
        {
            var IQueryableMethod = ThenByMethod;
            return CallIQueryableMethod<TSource>(source, keySelector, IQueryableMethod);
        }

        public static IOrderedQueryable<TSource> OrderByDescendingProperty<TSource>
            (this IQueryable<TSource> source, Expression<Func<TSource, object>> keySelector)
        {
            var IQueryableMethod = OrderByDescendingMethod;
            return CallIQueryableMethod<TSource>(source, keySelector, IQueryableMethod);
        }

        public static IOrderedQueryable<TSource> ThenByDescendingProperty<TSource>
            (this IOrderedQueryable<TSource> source, Expression<Func<TSource, object>> keySelector)
        {
            var IQueryableMethod = ThenByDescendingMethod;
            return CallIQueryableMethod<TSource>(source, keySelector, IQueryableMethod);
        }

        private static IOrderedQueryable<TSource> CallIQueryableMethod<TSource>(IQueryable<TSource> source, Expression<Func<TSource, object>> keySelector, MethodInfo IQueryableMethod)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "posting");

            //By default, use the passed in selector without modification
            LambdaExpression lambda = keySelector;//Expression.Lambda(keySelector, new[] { parameter });
            //Type returnType = typeof(object);

            //Linq To Entities fails if Convert Expression is being used. So, 
            Expression selector = null;
            if (!(keySelector.Body is MemberExpression)
                && keySelector.Body.NodeType == ExpressionType.Convert
                && ((UnaryExpression)keySelector.Body).Operand is MemberExpression)
            {
                selector = ((UnaryExpression)keySelector.Body).Operand as MemberExpression;
                lambda = Expression.Lambda(selector, keySelector.Parameters);
            }



            //var properties = propertyName.Split('.');
            //Expression orderByProperty = null;
            //foreach (var prop in properties)
            //{
            //    if (orderByProperty == null)
            //    {
            //        orderByProperty = Expression.Property(parameter, prop);
            //        continue;
            //    }
            //    orderByProperty = Expression.Property(orderByProperty, prop);
            //}


            //LambdaExpression lambda = Expression.Lambda(keySelector, new[] { parameter });

            MethodInfo genericMethod = IQueryableMethod.MakeGenericMethod
                (new[] { typeof(TSource), lambda.ReturnType });
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<TSource>)ret;
        }                
    }
}
