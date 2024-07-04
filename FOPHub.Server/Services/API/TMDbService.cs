using FOPHub.Library;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FOPHub.Server.Services.API
{
    /// <summary>
    /// Implementation for managing external API calls to TMDb.
    /// </summary>
    public class TMDbService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private readonly string _apiKey;

        public TMDbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "4fcf5aad7ab924ec7c3d743b059dfade";
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
