using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace MLM.API.DB.Specifications
{
    public class FetchStrategyBase<T> : IFetchStrategy<T> where T: class 
    {
        protected IList<Expression<Func<T, object>>> properties;

        public FetchStrategyBase()
        {
            properties = new List<Expression<Func<T, object>>>();
        }

        #region IFetchStrategy<T> Members

        public IEnumerable<Expression<Func<T, object>>> IncludePaths
        {
            get { return properties; }
        }

        public IFetchStrategy<T> Include(Expression<Func<T, object>> path)
        {
            properties.Add(path);
            return this;
        }

        //TODO: A string based path inclusion is not required immediately. A approach can come in later
        public IFetchStrategy<T> Include(string path)
        {
            throw new NotImplementedException("A string based path inclusion is not required immediately. A approach can come in later");
            //properties.Add(path);
            //return this;
        }

        #endregion
    }

    public static class Extensions
    {
        public static string ToPropertyName<T>(this Expression<Func<T, object>> selector)
        {            
            var me = selector.Body as MemberExpression;
            
            if (me == null)
            {
                throw new ArgumentException("MemberExpression expected.");
            }

            var propertyName = me.ToString().Remove(0, 2);
            return propertyName;
        }
    }

    public class CompositeFetchStrategy<T>: FetchStrategyBase<T> where T: class
    {
        public CompositeFetchStrategy(IFetchStrategy<T> lSide, IFetchStrategy<T> rSide)
        {
            this.properties = lSide.IncludePaths.Union(rSide.IncludePaths).ToList();            
        }
    }

    public class CompositeOrderStrategy<T> : OrderStrategyBase<T>
    {
        public CompositeOrderStrategy(IOrderStrategy<T> lSide, IOrderStrategy<T> rSide)
        {
            this.orderingKeySelectors = lSide.OrderingKeyProperties.Union(rSide.OrderingKeyProperties).ToList();
        }
    }
}
