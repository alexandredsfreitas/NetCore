using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SS.WebApp.MVC.Extensions;
using SS.WebApp.MVC.Services;

namespace SS.WebApp.MVC.Configuration
{
	public static class DependencyInjectionConfig
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddHttpClient<IAuthenticationService, AuthenticationService>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddScoped<IUser, AspNetUser>();
		}
	}
}

