using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SS.WebApp.MVC.Configuration
{
	public static class IdendityConfig
	{
		public static void AddIdentityConfiguration(this IServiceCollection services)
		{
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Login";
					options.AccessDeniedPath = "/erro/403";
				});
		}

		public static void UseIdentityConfiguration(this IApplicationBuilder app)
		{
			app.UseAuthentication();
			app.UseAuthorization();
		}
	}
}

