using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace App.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(string city)
        {
            // 1. Geocoding: Þehir adýndan koordinatlarý bul
            var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1&language=en&format=json";
            var geoResponse = await _httpClient.GetFromJsonAsync<GeoResponse>(geoUrl);

            if (geoResponse?.Results == null || !geoResponse.Results.Any())
                throw new Exception($"{city} þehri bulunamadý.");

            var location = geoResponse.Results.First();

            // 2. Forecast: Koordinatlarla hava durumunu çek
            var forecastUrl = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&longitude={location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&daily=temperature_2m_max,weathercode&timezone=auto";
            var forecastResponse = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(forecastUrl);

            if (forecastResponse?.Daily == null)
                return Enumerable.Empty<WeatherForecast>();

            // 3. Verileri Maple
            var results = new List<WeatherForecast>();
            for (int i = 0; i < Math.Min(5, forecastResponse.Daily.Time.Length); i++)
            {
                results.Add(new WeatherForecast
                {
                    Date = DateOnly.Parse(forecastResponse.Daily.Time[i]),
                    TemperatureC = (int)forecastResponse.Daily.Temperature2mMax[i],
                    Summary = MapWeatherCode(forecastResponse.Daily.Weathercode[i]),
                    Location = $"{location.Name}, {location.Country}"
                });
            }

            return results;
        }

        private string MapWeatherCode(int code)
        {
            return code switch
            {
                0 => "Clear Sky",
                1 or 2 or 3 => "Partly Cloudy",
                45 or 48 => "Foggy",
                51 or 53 or 55 => "Drizzle",
                61 or 63 or 65 => "Rainy",
                71 or 73 or 75 => "Snowy",
                80 or 81 or 82 => "Rain Showers",
                95 or 96 or 99 => "Thunderstorm",
                _ => "Cloudy"
            };
        }
    }

    public class GeoResponse { public List<GeoResult> Results { get; set; } }
    public class GeoResult { public string Name { get; set; } public string Country { get; set; } public float Latitude { get; set; } public float Longitude { get; set; } }
    public class OpenMeteoResponse { public DailyData Daily { get; set; } }
    public class DailyData 
    { 
        [System.Text.Json.Serialization.JsonPropertyName("time")]
        public string[] Time { get; set; } 
        
        [System.Text.Json.Serialization.JsonPropertyName("temperature_2m_max")]
        public float[] Temperature2mMax { get; set; } 
        
        [System.Text.Json.Serialization.JsonPropertyName("weathercode")]
        public int[] Weathercode { get; set; } 
    }
}
