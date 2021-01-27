using Newtonsoft.Json;

namespace MovieSite.Core.Model
{
    public class Movie
    {
        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Year { get; set; }
        
        [JsonProperty("imdbID")]
        public string ImdbID { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string Poster { get; set; }
    }
}
