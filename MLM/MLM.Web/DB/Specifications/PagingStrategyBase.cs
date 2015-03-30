using System.Collections.Generic;
namespace MLM.DB.Specifications
{
    public class PagingStrategyBase<T>: IPagingStrategy<T> where T:class
    {
        protected IOrderStrategy<T> orderStrategy;
        protected IList<IPagingSpecification<T>> pagingSpecs;

        public PagingStrategyBase()
            :this(1, 100)   //TODO: Default values can be configurable?
        {            
        }

        public PagingStrategyBase(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            orderStrategy = new OrderStrategyBase<T>();
            pagingSpecs = new List<IPagingSpecification<T>>();
        }

        public virtual IOrderStrategy<T> OrderStrategy
        {
            get
            {
                return orderStrategy;
            }
            set
            {
                orderStrategy = value;
            }
        }

        public virtual int PageNumber { get; set; }

        public virtual int PageSize { get; set; }


        public IEnumerable<IPagingSpecification<T>> PagingSpecifications
        {
            get { return pagingSpecs; }
        }

        public bool IsInCurrentPage(long position)
        {
            return (PageNumber - 1) * PageSize < position && position <= (PageNumber) * PageSize;
        }


        public void IncludePagingSpecification(IPagingSpecification<T> pagingSpec)
        {
            pagingSpecs.Add(pagingSpec);
        }
    }
}
