using MLM.DB.Specifications;
using MLM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM.Web.Controllers
{
    public class TopachiversRepository<T> :BaseRepository<T> where T :TopAchivar
    {
        public virtual IEnumerable<T> GetTopachivers()
        {
            return EntityRepository
                .GetMany(new TopAchivarsList<T>());
        }


       




        public class TopAchivarsList<T> : SpecificationBase<T> where T : TopAchivar
        {
            public TopAchivarsList()
            {
                this.predicate = tc => tc.IsDeleted == false;
            }
        }
    }
}