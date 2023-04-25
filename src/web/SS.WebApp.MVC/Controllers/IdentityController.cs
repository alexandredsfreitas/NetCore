using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SS.WebApp.MVC.Models;
using SS.WebApp.MVC.Services;
using IAuthenticationService = SS.WebApp.MVC.Services.IAuthenticationService;
using System.IdentityModel.Tokens.Jwt;

namespace SS.WebApp.MVC.Controllers
{
	public class IdentityController : MainController
	{
		IAuthenticationService _authenticationService;

		public IdentityController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[HttpGet]
		[Route("new-account")]
		public IActionResult Registration()
		{
			return View();
		}

        [HttpPost]
        [Route("new-account")]
        public async Task<IActionResult> Registration(UserRegistration userRegistration)
        {
            if (!ModelState.IsValid) return View(userRegistration);

            var response = await _authenticationService.Register(userRegistration);

            if (ResponseHasErrors(response.ResponseResult))
                return View(userRegistration);

            await PerformLogin(response);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)

        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(userLogin);

            var response = await _authenticationService.Login(userLogin);

            if (ResponseHasErrors(response.ResponseResult)) return View(userLogin);

            await PerformLogin(response);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task PerformLogin(UserLoginResponse response)
        {
            var token = GetFormattedToken(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private static JwtSecurityToken GetFormattedToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}

