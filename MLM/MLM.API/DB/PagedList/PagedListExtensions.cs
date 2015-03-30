using System.Collections.Generic;
using System.Linq;
using MLM.API.DB.Specifications;

namespace MLM.API.DB.PagedList
{
	/// <summary>
	/// Container for extension methods designed to simplify the creation of instances of <see cref="PagedList{T}"/>.
	/// </summary>
	public static class PagedListExtensions
	{
		/// <summary>
		/// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
		/// </summary>
		/// <typeparam name="T">The type of object the collection should contain.</typeparam>
		/// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
		/// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
		/// <param name="pageSize">The maximum size of any individual subset.</param>
		/// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
		/// <seealso cref="PagedList{T}"/>
		public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize)
		{
            if (pageNumber < 1)
                pageNumber = 1;
            if (pageSize < 1)
                pageSize = superset.Count();
			return new PagedList<T>(superset, pageNumber, pageSize);
		}

		/// <summary>
		/// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
		/// </summary>
		/// <typeparam name="T">The type of object the collection should contain.</typeparam>
		/// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
		/// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
		/// <param name="pageSize">The maximum size of any individual subset.</param>
		/// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
		/// <seealso cref="PagedList{T}"/>
		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
		{
            if (pageNumber < 1)
                pageNumber = 1;
            if (pageSize < 1)
                pageSize = superset.Count();
			return new PagedList<T>(superset, pageNumber, pageSize);
		}

        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="subset">The collection of objects already dividide into subset. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <param name="totalItemCount">The total item count (evaluated before hand)</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> subset, int pageNumber, int pageSize, int totalItemCount)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            if (pageSize < 1)
                pageSize = subset.Count();
            return new PagedList<T>(subset, pageNumber, pageSize, totalItemCount);
        }
        
	}
}