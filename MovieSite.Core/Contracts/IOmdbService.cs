using MovieSite.Core.Model;
using System.Threading.Tasks;

namespace MovieSite.Core.Contracts
{
    public interface IOmdbService
    {
        Task<MovieDetails> GetDetailsByImdbIdAsync(string query);
        Task<MovieList> GetListByTitleAsync(string query);
    }
}
