using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TravelGenie_WeB_APP.Server.Controllers;
using TravelGenie_WeB_APP.Server.Dtos;
using TravelGenie_WeB_APP.Server.Services;
using Xunit;




namespace YourNamespace.Tests.Controllers
{
    public class TravelControllerTests
    {


        [Fact]
        public async Task GetCountries_Returns_OkResult_With_CountryList()
        {
            // Arrange
            var fakeTravelService = new Mock<ITravel>();
            var expectedCountries = new List<string> { "Sweden", "Ethiopia", "United Arab Emerates" };
            fakeTravelService.Setup(x => x.GetCountryNames()).ReturnsAsync(expectedCountries);
            var controller = new TravelController(fakeTravelService.Object);

            // Act
            var result = await controller.GetCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedCountries, okResult.Value);
        }

        [Fact]
        public async Task GetCountries_Returns_500_When_Exception_Occurs()
        {
            // Arrange
            var fakeTravelService = new Mock<ITravel>();
            fakeTravelService.Setup(x => x.GetCountryNames()).ThrowsAsync(new Exception("Some error occurred"));
            var controller = new TravelController(fakeTravelService.Object);

            // Act
            var result = await controller.GetCountries();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async Task GetCities_Returns_OkResult_With_CityList()
        {
            // Arrange
            var fakeTravelService = new Mock<ITravel>();
            var expectedCities = new List<string> { "Stockholm", "Sundsvall", "Uppsala" };
            string country = "Sweden";
            fakeTravelService.Setup(x => x.GetCities(country)).ReturnsAsync(expectedCities);
            var controller = new TravelController(fakeTravelService.Object);

            // Act
            var result = await controller.GetCities(country);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedCities, okResult.Value);
        }

        [Fact]
        public async Task GetCities_Returns_500_When_Exception_Occurs()
        {
            // Arrange
            var fakeTravelService = new Mock<ITravel>();
            string country = "NamelessCountry";
            fakeTravelService.Setup(x => x.GetCities(country)).ThrowsAsync(new Exception("Some error occurred"));
            var controller = new TravelController(fakeTravelService.Object);

            // Act
            var result = await controller.GetCities(country);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetPlanDetail_Returns_ChatGptDto_For_Valid_Prompt()
        {
            // Arrange
            var prompt = "Can you recommend some restaurants in Paris?";
            var expectedResponse = new ChatGptDto
            {
                text = "Certainly! Here are some top-rated restaurants in Paris:\n1. Le Bernardin\n2. Eleven Madison Park"
            };

            var fakeHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };
            fakeHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var client = new HttpClient(fakeHttpMessageHandler.Object);
            var travelService = new Travel(client);

            // Act
            var result = await travelService.GetPlanDetail(prompt);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.text, result.text);
        }



    }
}
