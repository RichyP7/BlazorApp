using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static WeatherApp.Pages.FetchData;

namespace WeatherApp
{
    public class WeatherForeCastService
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private readonly Uri endpoint;

        public WeatherForeCastService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            endpoint = new Uri(configuration["WeatherendPoint"]);
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync()
        {

            var response = await this.client.GetAsync(endpoint.AbsoluteUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            WeatherForeCast7Timer weatherResult =  JsonSerializer.Deserialize<WeatherForeCast7Timer>(responseBody);
            var foreCasts = new List<WeatherForecast>();
            foreach (var item in weatherResult.dataseries)
            {
                foreCasts.Add(new WeatherForecast()
                {
                    Date = DateTime.Today.AddHours(3).AddHours(item.timepoint),
                    TemperatureF = item.temp2m,
                    WindSpeed = item.wind10m.speed,
                    WindDirection = item.wind10m.direction,
                    Humidity = item.rh2m,
                    Summary = GetPlainInformation(item)
                });
            }
            return foreCasts;

        }

        private string GetPlainInformation(WeatherForeCastTimePoint item)
        {
            return item.weather switch
            {
                var x when (x.Equals("clearday") ||x.Equals("clearnight")) => "Gesamtwolkenbedeckung unter 20%",
                var x when (x.Equals("pcloudyday") || x.Equals("pcloudynight")) => "Gesamtwolkenbedeckung zwischen 20%-60%",
                var x when (x.Equals("mcloudyday") || x.Equals("mcloudynight")) => "Gesamtwolkenbedeckung zwischen 60%-80%",
                var x when (x.Equals("humidday") || x.Equals("humidnight")) => "Feuchtigkeit über 90% mit Wolkenbedeckung unter 60%",
                var x when (x.Equals("lightrainday") || x.Equals("lightrainnight")) => "Niederschlagsrate weniger als 4mm/hr ,Bewölkung über 80%",
                var x when (x.Equals("oshowerday") || x.Equals("oshowernight")) => "Niederschlagsrate weniger als 4mm/hr Bewölkung zwischen 60%-80%",
                var x when (x.Equals("ishowerday") || x.Equals("ishowernight")) => "Niederschlagsrate weniger als 4mm/hr Bewölkung unter 60%",
                var x when (x.Equals("lightsnowday") || x.Equals("lightsnownight")) => "Niederschlagsrate weniger als 4mm/hr",
                var x when (x.Equals("rainday") || x.Equals("rainnight")) => "Niederschlagsrate über 4mm/hr",
                var x when (x.Equals("snowday") || x.Equals("snownight")) => "Niederschlagsrate über  4mm/hr",
                var x when (x.Equals("rainsnowday") || x.Equals("rainsnownight")) => "Schnee oder Eisregen",
                var x when (x.Equals("tsday") || x.Equals("tsnight")) => "Angehobener Index weniger als -5 mit einer Niederschlagsrate unter 4 mm / h",
                var x when (x.Equals("tsrainday") || x.Equals("tsrainnight")) => "Angehobener Index weniger als -5 mit einer Niederschlagsrate über 4 mm / h",
                _ => "unknown",
            };
        }
    }
    public class WeatherForeCast7Timer
    {
        public WeatherForeCast7Timer()
        {
            dataseries = new List<WeatherForeCastTimePoint>();
        }

        public string product { get; set; }
        public string init { get; set; }//datetimestring
        public IEnumerable<WeatherForeCastTimePoint> dataseries { get; set; }
    }
    public class WeatherForeCastTimePoint
    {
        public int timepoint { get; set; }
        public int cloudcover { get; set; }
        public int lifted_index { get; set; }
        public string prec_type { get; set; }
        public int prec_amount { get; set; }
        public int temp2m { get; set; }
        public string rh2m { get; set; }
        public string weather { get; set; }
        public WindForeCast wind10m { get; set; }
    }
    public class WindForeCast
    {
        public string direction { get; set; }
        public int speed { get; set; }
    }
}
