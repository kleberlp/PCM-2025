using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PCM.ADM.WEB.Startup))]

namespace PCM.ADM.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
