using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace WeatherApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configdict = new Dictionary<string, string>();
            configdict.Add( "WeatherendPoint", "http://www.7timer.info/bin/api.pl?lon=47.436919&lat=15.942476&product=civil&output=json");
            configdict.Add("Username", "");
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Configuration.AddInMemoryCollection(configdict);
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
