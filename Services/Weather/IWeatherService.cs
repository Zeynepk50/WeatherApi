using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services.Weather
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetForecastAsync(string city);
        Task<IEnumerable<WeatherForecast>> GetForecastByCoordsAsync(float lat, float lon, string locationName);
    }
}
