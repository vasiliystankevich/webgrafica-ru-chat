using System;
using System.Collections.Generic;
using Libraries.Core.Backend.WebApi;

namespace Modules.WebApi.Controllers.Messages
{
    public class UserMessage
    {
        public void InitOthner()
        {
            IsSelf = false;
        }

        public void InitSelf()
        {
            IsSelf = true;
        }

        public string UserName { get; set; }
        public string Content { get; set; }
        public bool IsSelf { get; set; }
        public string TimeMessage { get; set; }
    }

    public class GetMessagesResponse : OkResponse
    {
        public GetMessagesResponse(List<UserMessage> messages)
        {
            Messages = messages;
        }
        public List<UserMessage> Messages { get; set; }
    }
}
