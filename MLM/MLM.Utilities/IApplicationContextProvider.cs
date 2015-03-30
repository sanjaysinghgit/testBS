using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
    public interface IApplicationContextProvider
    {
        object GetContextObject(object key);
        TU GetContextObject<T, TU>(T key) where TU : IConvertible;
        void SetContextObject(object key, object value);
    }
}
