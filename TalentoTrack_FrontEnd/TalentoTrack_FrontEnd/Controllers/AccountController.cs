using Microsoft.AspNetCore.Mvc;

namespace TalentoTrack_FrontEnd.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
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
                ViewBag.message = "Login Successfully.";
            }
            else
            {
                ViewBag.message = response.ErrorMessage;
            }

            return View();
        }
    }
}
