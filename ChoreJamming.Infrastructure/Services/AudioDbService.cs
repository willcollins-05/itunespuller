using ChoreJamming.Domain;
using ChoreJamming.Domain.Models;
using Newtonsoft.Json.Linq;

namespace ChoreJamming.Infrastructure.Services;

public class AudioDbService : IMusicProvider
{
    private readonly HttpClient _http;

    private JToken? _previousResults;
    
    public AudioDbService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Song?> GetSongAsync(string query, bool remix)
    {
        const int numberOfSongsPulled = 10;
        var url = $"https://itunes.apple.com/search?term={query}&entity=song&limit={numberOfSongsPulled}";

        try
        {
            JToken? results;
            if (remix)
            {
                if (_previousResults == null || !_previousResults.HasValues)
                {
                    return null;
                }
                results = _previousResults;
            }
            else
            {
                var response = await _http.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                var jsonString = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(jsonString)) return null;

                var json = JObject.Parse(jsonString);
                results = json["results"];

                if (results == null || !results.HasValues) return null;
                _previousResults = results;
            }

            Random random = new Random();

            var item = results[random.Next(numberOfSongsPulled)];
            return new Song
            {
                Title = item["trackName"]?.ToString() ?? "Unknown",
                Artist = item["artistName"]?.ToString() ?? "Unknown",
                Album = item["collectionName"]?.ToString() ?? "Single",
                VideoUrl = item["previewUrl"]?.ToString() ?? "",
                ThumbnailUrl = item["artworkUrl100"]?.ToString().Replace("100x100", "600x600") ?? "",
                ReleaseDate = DateTime.Parse(item["releaseDate"]?.ToString() ?? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss")), 
                PrimaryGenreName = item["primaryGenreName"]?.ToString() ?? "Unknown",
            };
        }
        catch
        {
            return null;
        }
    }
}