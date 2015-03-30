using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class Product : MLMBaseEntity
    {
        //TODO: Do we need to map this product with the eCommerce product.

        public string Title { get; set; }
        public decimal MRP { get; set; }
        public decimal DPPrice { get; set; }
        /// <summary>
        /// Business Point
        /// </summary>
        public decimal BP { get; set; }
        
        /// <summary>
        /// Product category
        /// </summary>
        public ProductCategory Category { get; set; }
        
        /// <summary>
        /// Product Grade
        /// </summary>
        public ProductGrade Grade { get; set; }
    }

}
