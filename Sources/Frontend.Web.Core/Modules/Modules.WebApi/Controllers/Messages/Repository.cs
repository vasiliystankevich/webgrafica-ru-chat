using System;
using System.Linq;
using System.Web;
using Dal;
using log4net;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi;
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
            var encodedMessage = HttpUtility.HtmlEncode(request.Message);
            message.Init(userName, encodedMessage);
            Context.Messages.Add(message);
            Context.SaveChanges();
            return BaseResponse.Ok();
        }

        public GetMessagesResponse GetMessages(GetMessagesRequest request)
        {
            RemoveMessages();
            var username = HttpContext.Current.User.Identity.Name;
            var countMessages = Context.Messages.Count();
            if (request.CountLastMessages < 0) request.CountLastMessages = countMessages;
            if (request.CountLastMessages > countMessages) request.CountLastMessages = countMessages;
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

        private void RemoveMessages()
        {
            var deleteTime = DateTime.UtcNow.AddDays(-1);
            var deleteMessages = Context.Messages.Where(m => m.TimeMessage <= deleteTime).ToList();
            deleteMessages.ForEach(m => Context.Messages.Remove(m));
            Context.SaveChanges();
        }

        public IDalContext Context { get; set; }
    }
}