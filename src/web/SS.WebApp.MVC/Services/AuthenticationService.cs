using System;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using SS.WebApp.MVC.Extensions;
using SS.WebApp.MVC.Models;

namespace SS.WebApp.MVC.Services
{
	public class AuthenticationService : Service, IAuthenticationService
    {
		private readonly HttpClient _httpClient;

		public AuthenticationService(HttpClient httpClient, IOptions<AppSettings> settings)
		{
			httpClient.BaseAddress = new Uri(settings.Value.AuthenticationURL);

			_httpClient = httpClient;
		}

		public async Task<UserLoginResponse> Login(UserLogin userLogin)
		{
			var loginContent = GetContent(userLogin);

			var response = await _httpClient.PostAsync("/api/identity/Login", loginContent);

			if (!ProcessResponseErrors(response))
				return new UserLoginResponse { ResponseResult = await DeserializeResponseObject<ResponseResult>(response) };

			return await DeserializeResponseObject<UserLoginResponse>(response);
		}

        public async Task<UserLoginResponse> Register(UserRegistration userRegistration)
		{
			var registrationContent = GetContent(userRegistration);

			var response = await _httpClient.PostAsync("/api/identity/new-account", registrationContent);

            if (!ProcessResponseErrors(response))
                return new UserLoginResponse { ResponseResult = await DeserializeResponseObject<ResponseResult>(response) };

            return await DeserializeResponseObject<UserLoginResponse>(response);
        }
    }
}

