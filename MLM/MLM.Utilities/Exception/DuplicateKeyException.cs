using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Exception
{
    public class DuplicateKeyException : MLMException
    {
        public DuplicateKeyException()
            : base()
        {

        }

        public DuplicateKeyException(System.Exception ex)
            : base("Duplicate Keys exception while inserting record in database", ex)
        {

        }
    }
}
