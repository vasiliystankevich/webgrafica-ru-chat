using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.WebApi;
using Libraries.Core.Backend.WebApi.Repositories;
using Project.Kernel;

namespace Modules.WebApi.Controllers.Messages
{
    public interface IMessagesApiController
    {
        Task<HttpResponseMessage> SendMessage(SendMessageRequest request);
        Task<HttpResponseMessage> GetMessages(GetMessagesRequest request);
    }

    [RoutePrefix("messages")]
    public class MessagesApiController : BaseApiController<IMessagesApiRepository>, IMessagesApiController
    {
        public MessagesApiController(IMessagesApiRepository repository, IVersionRepository versionRepository, Wrapper<ILog> logger) : base(repository, versionRepository, logger)
        {
        }

        [Route("sendmessage")]
        [HttpPost]
        [Authorize(Roles = ERoles.User)]
        public Task<HttpResponseMessage> SendMessage(SendMessageRequest request)
        {
            return ExecuteAction(request, Repository.SendMessage);
        }

        [Route("getmessages")]
        [HttpPost]
        [Authorize(Roles = ERoles.User)]
        public Task<HttpResponseMessage> GetMessages(GetMessagesRequest request)
        {
            return ExecuteAction(request, Repository.GetMessages);
        }
    }
}
