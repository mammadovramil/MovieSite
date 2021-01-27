using Microsoft.Extensions.Options;
using MovieSite.Core.Contracts;
using MovieSite.Core.Entity.Settings;
using MovieSite.Core.Model;
using MovieSite.Core.Services;
using NUnit.Framework;

namespace MovieSite.Tests
{
    public class OmdbServiceTests
    {
        private readonly IOptions<OmdbAPISettings> _options = Options.Create(new OmdbAPISettings() { Key = "d50f4ccc", Url = "http://www.omdbapi.com/?apikey=" });

        IOmdbService _omdbService;

        [SetUp]
        public void Setup()
        {
            _omdbService = new OmdbService(_options);
        }

        [Test]
        public void Test_Imdb_Movie_List_Is_Not_Empty()
        {
            var movieList = _omdbService.GetListByTitleAsync("&s=hello");
            Assert.IsTrue(movieList.Result.Movies.Length > 0);
        }

        [Test]
        public void Test_Imdb_Movie_Details_Is_Not_Empty()
        {
            var movieDetails = _omdbService.GetDetailsByImdbIdAsync($"&i=tt3766394");
            Assert.IsTrue(movieDetails.Result.Title != null);
        }
    }
}