using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(F2022A5HKA.Startup))]

namespace F2022A5HKA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
