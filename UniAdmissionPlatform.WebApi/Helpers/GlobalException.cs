using System;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;

namespace UniAdmissionPlatform.WebApi.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalException : Exception
    {
        public ExceptionCode Code { get; set; }
        public string Message { get; set; }
        public object ErrorObject { get; set; }

        public GlobalException(ExceptionCode code, string message, object errorObject)
        {
            Code = code;
            Message = message;
            ErrorObject = errorObject;
        }

        public GlobalException(ExceptionCode code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}