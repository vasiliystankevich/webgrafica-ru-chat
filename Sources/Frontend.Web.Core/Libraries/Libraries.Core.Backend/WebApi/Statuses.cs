﻿namespace Libraries.Core.Backend.WebApi
{
    public class BaseResponseStatus
    {
        protected BaseResponseStatus(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static BaseResponseStatus Create(int code, string message)
        {
            return new BaseResponseStatus(code, message);
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
}
