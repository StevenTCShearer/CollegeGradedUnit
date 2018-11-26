using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ValueFurniture.Startup))]
namespace ValueFurniture
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
