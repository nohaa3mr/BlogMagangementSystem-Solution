using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using BlogMagangementSystem.Features.UserFeatures.Registeration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting;


public class CreateMemberHandlerTests
{
    private readonly Mock<GenericRepository<User>> _mockUserRepo;
    private readonly Mock<BaseRequestParameters> _mockParameters;

    public CreateMemberHandlerTests()
    {
        _mockUserRepo = new();
        _mockParameters = new();

    }
    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenEmailIsNotUnique()
    {
        
        // Arrange
        var user = new UserDto() { FirstName = "John", 
            LastName = "Doe", Username = "JohnDoe"
            , Email = "Test@gmail.com" ,
            Address="Cairo", 
            PhoneNumber =01234567878 ,
            Password = "P@ssw0rd123" , };
        var command =  new UserRegisterationCommand(user);
        var handler = new UserRegisterationCommandHandler(_mockParameters.Object , _mockUserRepo.Object);
        // Act

        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
       Assert.True(result.IsSuccess);


    }
}
