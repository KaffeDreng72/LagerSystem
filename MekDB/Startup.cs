using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MekDB.Startup))]
namespace MekDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
        }
    }
}
