using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MLM.Models;

namespace MLM.DB.Specifications
{
    public interface IOrderStrategy<T>
    {
        IEnumerable<KeyValuePair<Expression<Func<T, object>>, SortOrder>> OrderingKeyProperties { get; }

        IOrderStrategy<T> Include(Expression<Func<T, object>> keySelector, SortOrder sortOrder);

        //IOrderStrategy<T> Include(string keySelector);
    }
}
