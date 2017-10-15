using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Libraries.Core.Backend.Common
{
    public abstract class BaseController:Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture =
                            Thread.CurrentThread.CurrentUICulture =
                                new CultureInfo("ru-Ru");
            base.Initialize(requestContext);
        }

        protected Task<T> GetAsyncResult<T>(T result) where T:ActionResult
        {
            return Task<T>.Factory.StartNew(() => result);
        }

        public Task<ActionResult> GeneratorActionResult(string viewName)
        {
            return Task<ActionResult>.Factory.StartNew(() => View(viewName));
        }

        public Task<ActionResult> GeneratorActionResult<T>(string viewName, T model)
        {
            return Task<ActionResult>.Factory.StartNew(() => View(viewName, model));
        }
    }
}