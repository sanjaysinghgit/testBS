using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace MLM.API.DB.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T> where T: class 
    {
        protected IFetchStrategy<T> fetchStrategy;
        protected Expression<Func<T, bool>> predicate;
        protected Expression<Func<T,long,bool>> expr;

        protected SpecificationBase()
        {
            this.predicate = t => true; //By default, no filter needs to be applied
            fetchStrategy = new FetchStrategyBase<T>();
        }

        public Expression<Func<T, bool>> Predicate
        {
            get 
            { 
                return predicate; 
            }
        }

        public IFetchStrategy<T> FetchStrategy
        {
            get { return fetchStrategy; }
            set { fetchStrategy = value; }
        }


        public bool IsSatisifedBy(T entity)
        {
            return new[] { entity }.AsQueryable().Any(predicate);
        }

        public AndSpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public OrSpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public NotSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public ParenthesizedSpecification<T> Para()
        {
            return new ParenthesizedSpecification<T>(this);
        }
    }

    /// <summary>
    /// A specification to be used by adapter. This specification is not type-safe, and parameters, method names are passed as strings. 
    /// Thus, this class should be used carefully and only when absolutely necessary    
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //TODO : Find a type safe way to do this. Currently its based on reflection
    public abstract class AdapterSpecificationBase<T> : SpecificationBase<T> where T : class
    {
        protected string name;

        protected IDictionary<string, string> parameters;

        public AdapterSpecificationBase()
        {
            name = "";
            parameters = new Dictionary<string, string>();
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public IDictionary<string, string> Parameters
        {
            get
            {
                return parameters;
            }
        }
    }
}
