using WeatherApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Moq.Protected;
using System.Net;

namespace WeatherFunctionalTest
{
    [TestClass]
    public class ForeCastServiceTest
    {
        private const string ENDPOINT = "http://www.test.com/subroute";
        private Mock<IConfiguration> configMock;
        private Mock<HttpMessageHandler> clientMock;
        private HttpClient client;
        private WeatherForeCastService service;

        [TestInitialize]
        public void InitTest()
        {
            configMock = new Mock<IConfiguration>();

            clientMock = new Mock<HttpMessageHandler>();
            client = new HttpClient(clientMock.Object);
            configMock.SetupGet(x => x[It.Is<string>(s => s == "WeatherendPoint")]).Returns(ENDPOINT);
            service = new WeatherForeCastService(client, configMock.Object);
        }
        [TestMethod]
        public async Task GetForecastAsyncTest_CheckStub_CheckResult()
        {

            WeatherForeCast7Timer weatherForeCast7Timer = new WeatherForeCast7Timer();
            string jsonString;
            jsonString = JsonSerializer.Serialize(weatherForeCast7Timer);
            var msg = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            msg.Content = new StringContent(jsonString);
            clientMock.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var result = await service.GetForecastAsync();

            Assert.IsNotNull(result);
        }
    }
}
