using App.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(IProductService productService)
        {

            var products = productService.GetTopPriceProductAsync(count: 3).Result;
            //products.Data[2].Name = "awrwreas";   bunu engellememeiz lazým. Böyle bir durum olmamalý. Orijinal dönen datada deðiþiklik yapýlmaz. Çünkü bunlar referans tipi olduðu için datanýn kendisini deðil pointerý taþýyorlar
            //Böyle bir deðiþiklik baþka yerleri de etkiler. Eðer gerekiyorsa yeni bir nesne tanýmlanýr ve öyle yapýlýr.


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
