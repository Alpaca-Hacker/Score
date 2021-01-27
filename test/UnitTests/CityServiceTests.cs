using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Clients;
using CityInfo.Clients.Models;
using CityInfo.Domain.Results;
using CityInfo.Domain.Services;
using FunctionalTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;

namespace UnitTests
{
    [TestClass]
    public class CityServiceTests
    {
        [TestMethod]
        public async Task ReturnsSuccessTrue_WhenResponseIsOk()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            mockClient.TestAstroData = new AstroResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunset = "10:00 PM",
                        Sunrise = "08:00 AM"
                    }
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult());

            // Act
            var result = await sut.GetCityInfo("Test");

            // Assert

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task ReturnsSuccessFalse_WhenResponseIsError()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockClient.TestData = new CurrentResponse
            {
                Error = new Error
                {
                    Code = (int)WeatherApiErrorCodes.APIGeneralError,
                    Message = "Internal application error."
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult());

            // Act
            var result = await sut.GetCityInfo("Test");

            // Assert

            Assert.IsFalse(result.Success);
            Assert.AreEqual(500, result.ResponseCode);
        }

        [TestMethod]
        public async Task ReturnsNotFound_WhenResponseIsNotFound()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockClient.TestData = new CurrentResponse
            {
                Error = new Error
                {
                    Code = (int)WeatherApiErrorCodes.LocationNotFound,
                    Message = "No location found matching parameter 'q'"
                }
            };

            mockClient.TestAstroData = new AstroResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunset = "10:00 PM",
                        Sunrise = "08:00 AM"
                    }
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult());

            // Act
            var result = await sut.GetCityInfo("Test");

            // Assert

            Assert.IsFalse(result.Success);
            Assert.AreEqual(404, result.ResponseCode);
        }

        [TestMethod]
        public async Task ReturnsFahrenheit_WhenUseFahrenheit_IsTrue()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            mockClient.TestData = new CurrentResponse
            {
                CurrentWeather = new CurrentWeather{TempC = 10, TempF = 50}
            };

            mockClient.TestAstroData = new AstroResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunset = "10:00 PM",
                        Sunrise = "08:00 AM"
                    }
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult{Temperature = 10});

            // Act
            var result = await sut.GetCityInfo("Test", true);

            // Assert

            Assert.IsTrue(result.Success);
            Assert.AreEqual(50, result.Temperature);
        }

        [TestMethod]
        public async Task ReturnsCentigrade_WhenUseFahrenheit_IsFalse()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            mockClient.TestData = new CurrentResponse
            {
                CurrentWeather = new CurrentWeather { TempC = 10, TempF = 50 }
            };
            mockClient.TestAstroData = new AstroResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunset = "10:00 PM",
                        Sunrise = "08:00 AM"
                    }
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult { Temperature = 10 });

            // Act
            var result = await sut.GetCityInfo("Test", false);

            // Assert

            Assert.IsTrue(result.Success);
            Assert.AreEqual(10, result.Temperature);
        }

        [TestMethod]
        public async Task ReturnsCentigrade_WhenUseFahrenheit_IsMissing()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            mockClient.TestData = new CurrentResponse
            {
                CurrentWeather = new CurrentWeather { TempC = 10, TempF = 50 }
            };
            mockClient.TestAstroData = new AstroResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunset = "10:00 PM",
                        Sunrise = "08:00 AM"
                    }
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult { Temperature = 10 });

            // Act
            var result = await sut.GetCityInfo("Test");

            // Assert

            Assert.IsTrue(result.Success);
            Assert.AreEqual(10, result.Temperature);
        }

        [TestMethod]
        public async Task MapsSunriseAndSunset_WhenOkFromAstroEndpoint()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            mockClient.TestAstroData = new AstroResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunset = "10:00 PM",
                        Sunrise = "08:00 AM"
                    }
                }
            };

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult());

            // Act
            var result = await sut.GetCityInfo("Test");

            // Assert

            Assert.IsTrue(result.Success);
            Assert.AreEqual(mockClient.TestAstroData.Astronomy.Astro.Sunset, result.Sunset);
            Assert.AreEqual(mockClient.TestAstroData.Astronomy.Astro.Sunrise, result.Sunrise);
        }

        [TestMethod]
        public async Task ReturnsEmptySunriseAndSunset_WhenErrorFromAstroEndpoint()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockClient = new MockWeatherClient();
            var mockLogger = new Mock<ILogger>();

            var sut = new CityService(mockClient, mockMapper.Object, mockLogger.Object);

            mockClient.TestResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            mockClient.TestAstroData = new AstroResponse
            {
                Error = new Error
                {
                    Code = (int)WeatherApiErrorCodes.APIGeneralError,
                    Message = "Internal application error."
                }
            };

            mockClient.TestAstroResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            mockMapper.Setup(m => m.Map<GetCityInfoResult>(It.IsAny<CurrentResponse>()))
                .Returns(new GetCityInfoResult());

            // Act
            var result = await sut.GetCityInfo("Test");

            // Assert

            Assert.IsTrue(result.Success);
            Assert.IsTrue(string.IsNullOrEmpty(result.Sunset));
            Assert.IsTrue(string.IsNullOrEmpty(result.Sunrise));
        }
    }
}
