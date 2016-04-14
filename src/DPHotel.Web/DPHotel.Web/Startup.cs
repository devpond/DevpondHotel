using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DPHotel.Web.Startup))]
namespace DPHotel.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
