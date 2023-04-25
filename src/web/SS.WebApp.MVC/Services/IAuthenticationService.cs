using System;
using SS.WebApp.MVC.Models;

namespace SS.WebApp.MVC.Services
{
	public interface IAuthenticationService
	{
		Task<UserLoginResponse> Login(UserLogin userLogin);

		Task<UserLoginResponse> Register(UserRegistration userRegistration);
	}
}

