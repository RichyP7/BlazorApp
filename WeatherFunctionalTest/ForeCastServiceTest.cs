using DeliveryApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace WeatherFunctionalTest
{
    [TestClass]
    public class ForeCastServiceTest
    {
        WeatherForeCastService service = new WeatherForeCastService(new System.Net.Http.HttpClient());
        [TestMethod]
        public async Task GetForeCast()
        {
            await service.GetForecastAsync();
        }
    }
}
