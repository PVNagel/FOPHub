using FOPHub.Library;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FOPHub.Server.Services.Movie
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private readonly string _apiKey;

        /// <summary>
        /// Implementation for interacting with external movie data source: TMDb
        /// </summary>
        /// <param name="httpClient"></param>
        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "4fcf5aad7ab924ec7c3d743b059dfade"; // Consider moving this to a config file
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PopularMoviesResponse> GetPopularMoviesAsync()
        {
            string apiUrl = $"{BaseUrl}/movie/popular?api_key={_apiKey}&language=en-US&page=1";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var popularMoviesResponse = JsonSerializer.Deserialize<PopularMoviesResponse>(json, options);
                return popularMoviesResponse;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch popular movies. Status code: {response.StatusCode}");
            }
        }

        public async Task<MovieDetailsResponse> GetMovieByIdAsync(int movieId)
        {
            string apiUrl = $"{BaseUrl}/movie/{movieId}?api_key={_apiKey}&language=en-US";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var movieDetailsResponse = JsonSerializer.Deserialize<MovieDetailsResponse>(json, options);
                return movieDetailsResponse;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch movie details. Status code: {response.StatusCode}");
            }
        }
    }
}
