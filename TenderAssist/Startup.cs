using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TenderAssist.Startup))]
namespace TenderAssist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
