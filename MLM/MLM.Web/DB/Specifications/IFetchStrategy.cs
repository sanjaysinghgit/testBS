using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MLM.DB.Specifications
{
    public interface IFetchStrategy<T>
    {
        IEnumerable<Expression<Func<T, object>>> IncludePaths { get; }

        IFetchStrategy<T> Include(Expression<Func<T, object>> path);

        
        IFetchStrategy<T> Include(string path);

    }
}
