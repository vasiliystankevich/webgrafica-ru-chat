namespace Libraries.Core.Backend.WebApi
{
    public class BaseResponse
    {
        protected BaseResponse(int code, string message)
        {
            Status = BaseResponseStatus.Create(code, message);
            Version = GetVersion();
        }

        public string GetVersion() => "1.0.0.0";

        public static BaseResponse Create(int code, string message)
        {
            return new BaseResponse(code, message);
        }

        public static BaseResponse Ok(string message)
        {
            return new BaseResponse(200, message);
        }

        public static BaseResponse Ok()
        {
            return new BaseResponse(200, "Ok");
        }

        public static BaseResponse Bad(string message)
        {
            return new BaseResponse(400, message);
        }
        public BaseResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class OkResponse : BaseResponse
    {
        public OkResponse() : base(200, "Ok")
        {
        }
        public OkResponse(string message) : base(200, message)
        {
        }
    }
}
