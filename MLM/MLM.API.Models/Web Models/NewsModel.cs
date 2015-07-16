using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MLM.Models
{
    public class NewsModel : MLMBaseEntity
    {
        
        [Required]
        public String NewsTitle { get; set; }
        [Required]
        public string NewsDetails { get; set; }
       
    }
}