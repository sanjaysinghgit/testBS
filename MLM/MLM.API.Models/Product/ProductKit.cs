using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class ProductKit : MLMBaseEntity
    {
        public string Title { get; set; }
        
        /// <summary>
        /// Kit price - Kit could have smaller price than the total products.
        /// </summary>
        public decimal Price { get; set; }

        public IList<Product> Products { get; set; }
        
    }
}
