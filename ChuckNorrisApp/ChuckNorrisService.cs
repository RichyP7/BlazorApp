using Microsoft.Extensions.Configuration;
using System;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChuckNorrisApp
{
    public class ChuckNorrisService
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private readonly Uri endpoint;

        public ChuckNorrisService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            endpoint = new Uri(configuration["EndPoint"]);
        }

        public async Task<ChuckResponse> GetForecastAsync()
        {
            var response = await this.client.GetAsync(endpoint.AbsoluteUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ChuckResponse>(responseBody);
        }
    }
    public class ChuckResponse
    {
        public string Id { get; set; }
        public string value { get; set; }
        public string icon_url { get; set; }
        public string uri { get; set; }
    }
}

