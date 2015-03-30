using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MLM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MLM.Web.Models
{
    public class ApplicationUser : IdentityUser
    {

        // At the time of registration
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }

        //Can be assigned later.
        public string FatherName { get; set; }
        public string HusbandName { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public string Nominee { get; set; }
        public Relation Relation { get; set; }


        // FirstName & LastName will be stored in a different table called MyUserInfo
        public virtual Agent AgentInfo { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
    public enum Relation
    {
        Father = 1,
        Mother = 2,
        Brother = 3,
        Sister = 4,
        Husband = 5,
        Wife = 6,

    }


}