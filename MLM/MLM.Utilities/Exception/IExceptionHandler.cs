using System;
using System.Collections.Generic;

namespace MLM.Exception
{
    public interface IExceptionHandler
    {
        IDictionary<string, object> HandlerPropertyBag { get; }
        
        bool HandleException(MLMException exToHandle);
        bool HandleException(MLMException exToHandle, string message);
        //bool HandleException(SarasException exToHandle, ExceptionHandlerArgs handlerArgs);        
        //bool HandleException(SarasException exToHandle, string message, CustomMethodLevelPolicies policyName);
    }
}