#region Header Info
//-----------------------------------------------------------------------
// <copyright file="ILog.cs" company="Excel Soft India Pvt. Ltd.">
//  Copyright (c) Excel Soft India Pvt. Ltd. All rights reserved. Website: http://www.excelindia.com.
// </copyright>
// <summary> Interface for implementing the different Log provider.</summary>
// <createdby>Deesayya</createdby> 
// <createddate>9-Sept-2010</createddate>
// <revisionhistory>
//  <revision modifiedby='' modifieddate='' revisionno=''></revision>
// </revisionhistory>
//-----------------------------------------------------------------------
#endregion

#region Namespaces

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#endregion

namespace MLM.Logging
{
    /// <summary>
    /// Interface for implementing the different Log providers.
    /// </summary>
    public interface ILog
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }

        void ClearNDC();
        void LogDebug(object Message);
        void LogDebug(string Message);
        void LogDebug(object Message, System.Exception ex);
        void LogError(object Message);
        void LogError(object Message, System.Exception ex);
        void LogFatal(object Message);
        void LogFatal(object Message, System.Exception ex);
        void LogInfo(object Message);
        void LogInfo(string Message);
        void LogInfo(object Message, System.Exception ex);
        void LogInfoWithoutSession(object Message);
        void LogTrack(string PageName, string EventName);
        void LogTrack(string Message, string PageName, string EventName);
        void LogWarn(object Message);
        void LogWarn(object Message, System.Exception ex);        
    }
}

