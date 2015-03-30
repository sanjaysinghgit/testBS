using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM.Models
{
    public enum UserSessionStatus
    {
        Valid,
        InvalidOrExpired,
        Unknown
    }
}