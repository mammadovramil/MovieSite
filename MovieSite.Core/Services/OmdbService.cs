using Microsoft.Extensions.Options;
using MovieSite.Core.Contracts;
using MovieSite.Core.Entity.Settings;
using MovieSite.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MovieSite.Core.Services
{
    public class OmdbService : IOmdbService
    {
        private readonly IOptions<OmdbAPISettings> _oMDbAPISettings;

        public OmdbService(IOptions<OmdbAPISettings> oMDbAPISettings)
        {
            _oMDbAPISettings = oMDbAPISettings;
        }

        public async Task<MovieDetails> GetDetailsByImdbIdAsync(string query)
        {
			var data = await GetOmdbDataAsync<MovieDetails>(query);
			return data;
		}

        public async Task<MovieList> GetListByTitleAsync(string query)
        {
			var data = await GetOmdbDataAsync<MovieList>(query);
			return data;
        }

		private async Task<T> GetOmdbDataAsync<T>(string query)
		{
			var key = _oMDbAPISettings.Value.Key;
			var url = _oMDbAPISettings.Value.Url + key;

            using var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client
                .GetAsync($"{url}{query}")
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    var currentError = args.ErrorContext.Error.Message;
                    args.ErrorContext.Handled = true;
                }
            });
        }
	}
}
