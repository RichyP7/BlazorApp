using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            AddServices(builder);

            await builder.Build().RunAsync();
        }

        private static void AddServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddSingleton<WeatherForeCastService>();
            builder.Services.AddBaseAddressHttpClient();
        }
    }
}
