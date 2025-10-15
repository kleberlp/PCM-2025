using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PCM.WEB.Startup))]

namespace PCM.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
