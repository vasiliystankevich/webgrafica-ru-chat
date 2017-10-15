using Libraries.Core.Backend.WebApi;

namespace Modules.WebApi.Controllers.Messages
{
    public class SendMessageRequest : BaseRequest
    {
        public string Message { get; set; }
    }

    public class GetMessagesRequest : BaseRequest
    {
        public int CountLastMessages { get; set; }
    }
}