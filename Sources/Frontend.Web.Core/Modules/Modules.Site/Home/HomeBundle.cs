using Libraries.Core.Backend.Common;

namespace Modules.Site.Home
{
    public class HomeBundle:ModuleBaseBundleConfig
    {
        public override void RegisterBundles()
        {
            Action("Home","Index");
        }
    }
}
