using Microsoft.Owin;
using Owin;
using Project2;

[assembly: OwinStartup(typeof(Startup))]
namespace Project2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //  var chathub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            // GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => typeof(IdentityDbContext));

            app.MapSignalR();
        }
    }
}
