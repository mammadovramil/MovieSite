using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieSite.Core.Model
{
    public class MovieList
    {
        [JsonProperty("Search")]
        public Movie[] Movies { get; set; }

        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }

        [JsonProperty]
        public string Response { get; set; }

        [JsonProperty]
        public string Error { get; set; }
    }
}
