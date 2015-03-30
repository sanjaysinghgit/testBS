using MLM.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MLM.DB.Specifications
{
    public class OrderStrategyBase<T>: IOrderStrategy<T>
    {
        protected IList<KeyValuePair<Expression<Func<T, object>>, SortOrder>> orderingKeySelectors;

        public OrderStrategyBase()
        {
            orderingKeySelectors = new List<KeyValuePair<Expression<Func<T, object>>, SortOrder>>();
        }

        public IEnumerable<KeyValuePair<Expression<Func<T, object>>, SortOrder>> OrderingKeyProperties
        {
            get { return this.orderingKeySelectors; }
        }

        public IOrderStrategy<T> Include(Expression<Func<T, object>> keySelector, SortOrder sortOrder)
        {
            orderingKeySelectors.Add(new KeyValuePair<Expression<Func<T, object>>, SortOrder>(keySelector, sortOrder));
            return this;
        }

        //public IOrderStrategy<T> Include(string keySelector)
        //{
        //    orderingKeySelectors.Add(keySelector);
        //    return this;
        //}
    }
}
