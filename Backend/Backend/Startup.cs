using Microsoft.Owin;
using Owin;
using Backend.Data;

[assembly: OwinStartup(typeof(Backend.Startup))]

namespace Backend
{
	public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			// Настраиваем аутентификацию приложения
            ConfigureAuth(app);

			app.CreatePerOwinContext<TransactionsRepository>(() => new TransactionsRepository());

			app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}
