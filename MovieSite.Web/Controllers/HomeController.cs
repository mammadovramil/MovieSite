using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSite.Core.Contracts;
using MovieSite.Web.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using MovieSite.Core.Model;

namespace MovieSite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOmdbService _omdbService;

        public HomeController(ILogger<HomeController> logger, IOmdbService omdbService)
        {
            _logger = logger;
            _omdbService = omdbService;
        }

        public async Task<IActionResult> Index()
        {
            var movieList = await _omdbService.GetListByTitleAsync("&s=hello");

            if (movieList.Movies != null && movieList.Movies.Length > 0)
            {
                return View(movieList);
            }
            else
            {
                return View(new MovieList { Movies = null });
            }
        }

        public async Task<IActionResult> Information(string id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var movieDetails = await _omdbService.GetDetailsByImdbIdAsync($"&i={id}");
            if (movieDetails != null)
            {
                return View(movieDetails);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
