using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using MLM.API.Models;
using MLM.API.DB.Specifications;
using MLM.API.DB.PagedList;

namespace MLM.API.DB
{
    public interface IRepository<T> where T : MLMBaseEntity
    {
        T Add(T entity);
        IEnumerable<T> Add(IEnumerable<T> entity);
        T Update(T entity);
        T Delete(T entity);
        IEnumerable<T> Delete(ISpecification<T> specification);
        T GetById(long id);
        T GetById(string id);
         
        T Get(ISpecification<T> specification);

        TDerived Get<TDerived>(ISpecification<TDerived> specification)
            where TDerived : class, T;

        IQueryable<T> GetAll();

        IEnumerable<T> GetMany(ISpecification<T> specification);
        IEnumerable<T> GetMany(ISpecification<T> specification, params Expression<Func<T, object>>[] fetchStrategies);
        IPagedList<T> GetMany(ISpecification<T> specification, IPagingStrategy<T> pagingStrategy);

        IEnumerable<TDerived> GetMany<TDerived>(ISpecification<TDerived> specification)
            where TDerived : class, T;
        
        bool Matches(ISpecification<T> spec);

        Int64 Count(ISpecification<T> spec);

        IEnumerable<long> GetIds(ISpecification<T> specification);

        IEnumerable<long> GetIds<TDerived>(ISpecification<TDerived> specification) where TDerived : class,T;

        IEnumerable<T> GetMany(IEnumerable<long> ids);

        /// <summary>
        /// Method returns IQuerable so that query does not fetches extra data if not required.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<T> GetAll(ISpecification<T> specification);

        IEnumerable<T> ExecuteStoreQuery<T>(string sql, params object[] parameters);
    }
}
