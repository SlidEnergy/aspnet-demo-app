using System.Web.Http;

namespace Backend
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			// Настраиваем маршрутизацию для webapi
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}
