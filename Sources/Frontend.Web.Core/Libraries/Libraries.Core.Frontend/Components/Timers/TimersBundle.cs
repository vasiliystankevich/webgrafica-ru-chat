using Libraries.Core.Backend.Common;

namespace Libraries.Core.Frontend.Components.Timers
{
    public class TimersBundle : ComponentBaseBundleConfig
    {
        public TimersBundle() : base("Timers")
        {
        }

        public override void RegisterBundles()
        {
            RegisterJs($"{BaseComponentName}", new[]
            {
                "~/Libraries/Libraries.Core.Frontend/Components/Timers/jquery.timers.js",
            });
        }
    }
}
