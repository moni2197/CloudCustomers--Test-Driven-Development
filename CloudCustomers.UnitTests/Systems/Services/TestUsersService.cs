using CloudCustomers.API.Models;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCustomers.UnitTests.Systems.Services
{
    public class TestUsersService
    {
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
        {
            //Arrange
            //create moq http client in a different class(HELPERS)
            var expectedResponse = UsersFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResouceList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var sut = new UsersService(httpClient);

            //Act

            await sut.GetAllUsers();

            //Assert
            //Verify that http request is made. 
            //We have access to HttpClient Object. So create a client and pass it as dependency to service layer.
            //addHttpClient in program.cs for UsersService to get Http Client every time it is used.

            handlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());

        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnsListOfUsers()
        {
            //Arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>.SetupReturn404(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var sut = new UsersService(httpClient);

            //Act
            var result = await sut.GetAllUsers();

            //Assert
            result.Count.Should().Be(0);
        }
    }
}
