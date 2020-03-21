using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ChuckNorrisApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configdict = new Dictionary<string, string>();
            configdict.Add("EndPoint", "https://api.chucknorris.io/jokes/random");
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Configuration.AddInMemoryCollection(configdict);
            AddServices(builder);

            await builder.Build().RunAsync();
        }

        private static void AddServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddSingleton<ChuckNorrisService>();
            builder.Services.AddBaseAddressHttpClient();
        }
    }
}
