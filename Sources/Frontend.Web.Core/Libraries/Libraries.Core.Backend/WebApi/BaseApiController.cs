using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Schema;
using log4net;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi.Repositories;
using Newtonsoft.Json;
using Project.Kernel;

namespace Libraries.Core.Backend.WebApi
{
    public class BaseApiController<TRepository>:ApiController, IRepository<TRepository>
    {
        public BaseApiController(TRepository repository, IVersionRepository versionRepository, Wrapper<ILog> logger)
        {
            Repository = repository;
            Logger = logger;
            VersionRepository = versionRepository;
        }

        public async Task<HttpResponseMessage> ContentGenerator<T>(T model)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var content = JsonConvert.SerializeObject(model);
            response.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }

        public async Task<HttpResponseMessage> ExecuteAction<TRequest, TResponse>(TRequest request, Func<TRequest, TResponse> action) where TRequest:BaseRequest
        {
            Task<HttpResponseMessage> result;
            try
            {
                request.Verify(() => string.Compare(request.Version, VersionRepository.Version, StringComparison.OrdinalIgnoreCase) != 0, "invalid version of the request");
                var model = action(request);
                result = ContentGenerator(model);
            }
            catch (Exception exception)
            {
                Logger.Instance.Error(exception);
                var model = BaseResponse.Create(400, exception.Message);
                result = ContentGenerator(model);
            }
            return await result;
        }

        public TRepository Repository { get; set; }
        public Wrapper<ILog> Logger { get; set; }
        public IVersionRepository VersionRepository { get; set; }
    }
}
