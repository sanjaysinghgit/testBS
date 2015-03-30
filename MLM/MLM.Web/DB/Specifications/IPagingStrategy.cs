using System.Collections.Generic;
using System;
namespace MLM.DB.Specifications
{
    public interface IPagingStrategy<T> where T:class
    {
        IOrderStrategy<T> OrderStrategy { get; set; }

        int PageNumber { get; set; }

        int PageSize { get; set; }


        /// <summary>
        /// Specifications that describe the records to be inserted at specific positions in the result set
        /// </summary>
        IEnumerable<IPagingSpecification<T>> PagingSpecifications { get; }

        void IncludePagingSpecification(IPagingSpecification<T> pagingSpec);

        bool IsInCurrentPage(long position);
    }

    /// <summary>
    /// Defines a specification that is used to retrieve the record at a specific position.
    /// </summary>
    /// <typeparam name="T">The type for which paging is being applied</typeparam>
    public interface IPagingSpecification<T> where T:class
    {
        //TODO: Instead of top and bottom, a generic positionevaluator is desirable, for e.g. to put a record at last but position
        /// <summary>
        /// Evaluates the position at which the specification is to be applied.
        /// Takes pagingStrategy and totalItemCount as input and returns the position at which this specification needs to be applied
        /// </summary>
        //Func<IPagingStrategy<T>, long, long> PostionEvaluator {get; set;}

        PagingPosition PagingPosition { get; set; }

        ISpecification<T> PagingSpecificationAtPosition {get; set;}
    }

    public enum PagingPosition
    {
        Top,
        Bottom
    }

    public class PagingSpecificationBase<T> : IPagingSpecification<T> where T : class
    {

        //public Func<IPagingStrategy<T>, long, long> PostionEvaluator { get; set; }
        public PagingPosition PagingPosition { get; set; }

        public ISpecification<T> PagingSpecificationAtPosition { get; set; }
    }
}
