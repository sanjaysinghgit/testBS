#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace MLM.Exception
{
    /// <summary>
    /// Base Exception class for supporting the range of exceptions.
    /// </summary>
    [Serializable]
    public class MLMException : System.Exception
    {
        #region Constants
        /// <summary>
        /// Constant string for string formating.
        /// </summary>
        private const string Format = "{0}{1}{2}";
        #endregion

        /// <summary>
        /// ErrorLevel member variable, which holds the level of the specified exception.
        /// </summary>
        private string _innerStackTrace;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SarasException class.
        /// </summary>
        public MLMException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SarasException class.
        /// </summary>
        /// <param name="message">Message text describing the exception.</param>
        /// <param name="ex">Inner exception object.</param>
        public MLMException(string message, System.Exception ex)
            : base(message, ex)
        {
            if (ex != null)
            {
                this._innerStackTrace = ex.StackTrace;
                this.Source = ex.Source;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SarasException class.
        /// </summary>
        /// <param name="message">Message text describing the exception.</param>
        public MLMException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SarasException class. object for deserialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Serialiation context.</param>
        protected MLMException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this._innerStackTrace = info.GetString("innerStackTrace");
            this.ErrorCode = info.GetString("ErrorCode");
            this.RowNumber = info.GetInt32("RowNumber");
        }

        #endregion

        #region StackTrace
        /// <summary>
        /// Get the stack trace from the original
        /// exception.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string StackTrace
        {
            get
            {
                return string.Format(Format, this._innerStackTrace, Environment.NewLine, base.StackTrace);
            }
        }
        #endregion

        #region  Properties

        #region Property : ErrorCode
        /// <summary>
        /// Gets or sets the ErrorCode property, returns the errorCode for the error specified.
        /// </summary>
        public string ErrorCode { get; set; }
        #endregion

        #region  Property : RowNumber

        /// <summary>
        /// Gets or sets the Property to set RowNumber.
        /// </summary>         
        public int RowNumber { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the Innser stack trace.
        /// </summary>
        /// 
        internal string InnerStackTrace
        {
            get
            {
                return this._innerStackTrace;
            }

            set
            {
                this._innerStackTrace = value;
            }
        }
        #endregion

        #region Property : ErrorMessage
        /// <summary>
        /// Gets or sets the ErrorCode property, returns the errorCode for the error specified.
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion
    }
}
