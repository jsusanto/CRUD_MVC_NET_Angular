using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRUD_MVC_NET_Angular.Startup))]
namespace CRUD_MVC_NET_Angular
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
