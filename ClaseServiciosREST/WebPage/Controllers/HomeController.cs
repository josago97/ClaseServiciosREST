using Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebPage.Models;

namespace WebPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("/")]
        [HttpGet("/index.html")]
        public async Task<IActionResult> Index()
        {
            UserProfile user = await _apiService.GetUserAsync();

            if (user != null)
            {
                MessageResponse[] messages = (await _apiService.GetMessagesAsync())
                    .OrderByDescending(m => m.CreatedTime).ToArray();

                ViewData["User"] = user;
                ViewData["Messages"] = messages;

                if (string.IsNullOrEmpty(user.Image)) user.Image = "/images/default-user-image.png";
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
