using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM.Models
{
    public class LoginToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}