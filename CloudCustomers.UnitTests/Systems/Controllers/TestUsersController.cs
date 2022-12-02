using CloudCustomers.API.Controllers;
using CloudCustomers.API.Models;
using CloudCustomers.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CloudCustomers.UnitTests.Systems.Controllers;

public class TestUsersController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange : Setup our systems.
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(mockUsersService.Object);

        //Act : Call some method that we want to test.
        var result = (OkObjectResult)await sut.Get();


        //Assert : Make assertion about the outcome or action.
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokeUserServiceExactlyOnce()
    {
        //Arrange : Setup our systems.
        var mockUsersService = new Mock<IUsersService>();

        //Setup methods on the mock and control behavior of the dependencies to follow particular expected path that relates to this specefic test.
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());


        var sut = new UsersController(mockUsersService.Object);

        //Act : Call some method that we want to test.
        var result = await sut.Get();


        //Assert : Make assertion about the outcome or action. Need to make assertion that our userservice is invoked as expected.
        mockUsersService.Verify(s => s.GetAllUsers(), Times.Once());
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {
        var mockUsersService = new Mock<IUsersService>();

        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(mockUsersService.Object);

        var result = await sut.Get();

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
        
    }

    [Fact]
    public async Task Get_OnNoUsersFound_Returns404()
    {
        var mockUsersService = new Mock<IUsersService>();

        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        var result = await sut.Get();

        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);

    }


}