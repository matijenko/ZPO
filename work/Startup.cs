using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(work.Startup))]
namespace work
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
