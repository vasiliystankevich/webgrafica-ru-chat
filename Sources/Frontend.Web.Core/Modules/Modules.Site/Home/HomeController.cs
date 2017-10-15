using System.Threading.Tasks;
using System.Web.Mvc;
using Libraries.Core.Backend.Common;

namespace Modules.Site.Home
{
    public interface IHomeController
    {
        Task<ActionResult> Index();
    }

    [Authorize]
    public class HomeController:BaseController, IHomeController
    {
        public async Task<ActionResult> Index()
        {
            return await GeneratorActionResult("~/Home/Index/action.cshtml");
        }
    }
}