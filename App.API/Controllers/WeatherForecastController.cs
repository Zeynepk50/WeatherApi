using App.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherForecastController(IWeatherService weatherService, IConfiguration configuration, HttpClient httpClient)
        {
            _weatherService = weatherService;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get([FromQuery] string? city, [FromQuery] float? lat, [FromQuery] float? lon, [FromQuery] string? locationName)
        {
            try 
            {
                if (lat.HasValue && lon.HasValue)
                {
                    var forecast = await _weatherService.GetForecastByCoordsAsync(lat.Value, lon.Value, locationName ?? "Selected Location");
                    return Ok(forecast);
                }

                var location = city ?? _configuration["AppSettings:DefaultCity"] ?? "Istanbul";
                var cityForecast = await _weatherService.GetForecastAsync(location);
                return Ok(cityForecast);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                return Ok(new List<object>());

            try 
            {
                var url = $"https://geocoding-api.open-meteo.com/v1/search?name={query}&count=5&language=en&format=json";
                var response = await _httpClient.GetFromJsonAsync<GeoResponse>(url);
                return Ok(response?.Results ?? new List<GeoResult>());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
