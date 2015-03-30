using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLM.Models
{
   public class Student
    {
        public Student()
        {
            
        }
        [Key]
        [Column(Order = 1)]
        public int StudentKey1 { get; set; }

        [Key]
        [Column(Order = 2)]
        public int StudentKey2 { get; set; }

        public string StudentName { get; set; }
        


    }
}
