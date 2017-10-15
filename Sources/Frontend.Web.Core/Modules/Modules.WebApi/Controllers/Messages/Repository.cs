using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Dal;
using log4net;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi;
using Microsoft.Practices.ObjectBuilder2;
using Project.Kernel;

namespace Modules.WebApi.Controllers.Messages
{
    public interface IMessagesApiRepository
    {
        BaseResponse SendMessage(SendMessageRequest request);
        GetMessagesResponse GetMessages(GetMessagesRequest request);
        IDalContext Context { get; set; }
    }

    public class MessagesApiRepository : BaseRepository, IMessagesApiRepository
    {
        public MessagesApiRepository(IWrapper<ILog> logger, IDalContext context) : base(logger)
        {
            Context = context;
        }

        public BaseResponse SendMessage(SendMessageRequest request)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var user = Context.Accounts.FirstOrDefault(u => u.AccountName == userName);
            user.LastActivity=DateTime.UtcNow;
            var message = Context.Messages.Create();
            message.Init(userName, request.Message);
            Context.Messages.Add(message);
            Context.SaveChanges();
            return BaseResponse.Ok();
        }

        public GetMessagesResponse GetMessages(GetMessagesRequest request)
        {
            var username = HttpContext.Current.User.Identity.Name;
            var countMessages = Context.Messages.Count();
            var countSkipMessage = countMessages - request.CountLastMessages;
            if (countSkipMessage <= 0) countSkipMessage = 0;
            var messages = Context.Messages.OrderBy(m => m.TimeMessage).Skip(countSkipMessage)
                .ToList()
                .Select(m =>
                {
                    var userMessage = new UserMessage
                    {
                        UserName = m.UserName,
                        TimeMessage = m.TimeMessage.ToString("f"),
                        Content = m.Content
                    };
                    if (m.UserName == username) userMessage.InitSelf();
                    else userMessage.InitOthner();
                    return userMessage;
                }).ToList();
            return new GetMessagesResponse(messages);
        }

        public IDalContext Context { get; set; }
    }
}