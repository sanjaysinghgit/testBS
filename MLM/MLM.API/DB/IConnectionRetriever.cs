using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MLM.API.DB
{
    public interface IConnectionRetriever
    {
        String GetConnectionStringName();
    }

    public class ConnectionRetriever : IConnectionRetriever
    {

        public string GetConnectionStringName()
        {
            return "MLMCon";
        }
    }

}
