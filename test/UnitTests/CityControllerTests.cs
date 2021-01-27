using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CityInfo.API.Controllers;
using CityInfo.Contracts.Requests;
using CityInfo.Domain.Queries;
using CityInfo.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests
{
    [TestClass]
    public class CityControllerTests
    {
        [TestMethod]
        public async Task ReturnsBadRequestResult_WhenCityNameContainsDigits()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            var sut = new CityController(mockMediator.Object, mockMapper.Object);

            var request = new CityInfoRequest{CityName = "TEST123"};
            // Act
            var result = await sut.GetCityInfo(request);

            // Assert
           Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequestResult_WhenCityNameContainsColon()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            var sut = new CityController(mockMediator.Object, mockMapper.Object);

            var request = new CityInfoRequest { CityName = "London:Hammersmith" };
            // Act
            var result = await sut.GetCityInfo(request);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ReturnsNotFoundResult_WhenCityNotFound()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            mockMediator.Setup(m => m.Send(It.IsAny<GetCityInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetCityInfoResult {Success = false, ResponseCode = 404});

            var sut = new CityController(mockMediator.Object, mockMapper.Object);

            var request = new CityInfoRequest { CityName = "London" };
            // Act
            var result = await sut.GetCityInfo(request);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task ReturnsServerError_WhenApiSendsError()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            mockMediator.Setup(m => m.Send(It.IsAny<GetCityInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetCityInfoResult { Success = false, ResponseCode = 500 });

            var sut = new CityController(mockMediator.Object, mockMapper.Object);

            var request = new CityInfoRequest { CityName = "London" };
            // Act
            var result = await sut.GetCityInfo(request);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            var response = (ObjectResult) result.Result;
            Assert.AreEqual(500, response.StatusCode);
        }
    }
}
