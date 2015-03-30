using System;
using System.Linq.Expressions;

namespace MLM.API.DB.Specifications
{
    public interface ISpecification<T> where T:class 
    {
        Expression<Func<T, bool>> Predicate { get; }

        IFetchStrategy<T> FetchStrategy { get; set; }

        bool IsSatisifedBy(T entity);

        AndSpecification<T> And(ISpecification<T> specification);

        OrSpecification<T> Or(ISpecification<T> specification);

        NotSpecification<T> Not();
    }
}
