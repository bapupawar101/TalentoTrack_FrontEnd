using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TalentoTrack_FrontEnd.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}

        public IActionResult Singout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "account");
        }

        [HttpPost]
		public IActionResult Login(string username, string password)
		{
			TralentoTrack_API.TralentoTrackClient client = new TralentoTrack_API.TralentoTrackClient("https://localhost:7050/", new HttpClient());
			var response = client.LoginAsync(new TralentoTrack_API.LoginRequest()
			{
				Username = username,
				Password = password
			}).Result;

			if (response.Success)
			{
				if (response.User != null)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, username),
						new Claim(ClaimTypes.NameIdentifier, response.User.Id.ToString()),
						new Claim(ClaimTypes.Role, response.User.Role.ToString()),
						new Claim("FullName", $"{response.User.FirstName} {response.User.LastName}")
					};

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("dashboard", "admin");
				}
			}
			else
			{
				ViewBag.message = response.ErrorMessage;
			}

			return View();
		}
	}
}
