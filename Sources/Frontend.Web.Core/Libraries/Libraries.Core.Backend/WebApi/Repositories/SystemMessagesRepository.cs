using Dal;
using log4net;
using Libraries.Core.Backend.Common;
using Project.Kernel;

namespace Libraries.Core.Backend.WebApi.Repositories
{
    public interface ISystemMessagesRepository
    {
        void Send(string message);
        IDalContext Context { get; set; }
    }
    public class SystemMessagesRepository:BaseRepository, ISystemMessagesRepository
    {
        public SystemMessagesRepository(IWrapper<ILog> logger, IDalContext context) : base(logger)
        {
            Context = context;
        }

        public void Send(string message)
        {
            var systemMessage = Context.Messages.Create();
            systemMessage.Init("Система", message);
            Context.Messages.Add(systemMessage);
            Context.SaveChanges();
        }

        public IDalContext Context { get; set; }
    }
}