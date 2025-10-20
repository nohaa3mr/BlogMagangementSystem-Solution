using BlogMagangementSystem.Common.Context;
using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.JWT_Service;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using BlogMagangementSystem.Features.PostFeatures.AddPostFeature;
using BlogMagangementSystem.Features.PostFeatures.UpdatePostFeature;
using BlogMagangementSystem.Features.UserFeatures.GetUserRoleByUserID.Query;
using BlogMagangementSystem.Features.UserFeatures.HashingAlgorithm;
using BlogMagangementSystem.Features.UserFeatures.Login;
using BlogMagangementSystem.Features.UserFeatures.Registeration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace UnitTesting;
[TestCaseOrderer("UnitTesting.PriorityOrderer", "UnitTesting")]

public class CreateMemberHandlerTests
{
    private readonly IConfiguration _mockConfiguration = new Mock<IConfiguration>().Object;
    private readonly BlogDbContext _dbContext;
    private readonly DbContextOptions<BlogDbContext> _options;

    public CreateMemberHandlerTests()
    {
        _options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: "testDB")
            .Options;

        _dbContext = new BlogDbContext(_options);
    }
    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenEmailIsNotUnique()
    {
        // Arrange
        var userRepo = new GenericRepository<User>(_dbContext);
        var jwtService = new JWTService(_mockConfiguration);
        var parameters = new BaseRequestParameters(
            new Mock<UserNameHasher>().Object,
            new Mock<PasswordHasher>().Object,
            jwtService
        );


        var user = new UserDto()
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "JohnDoe",
            Email = "Test@gmail.com",
            Address = "Cairo",
            PhoneNumber = 01234567878,
            Password = "P@ssw0rd123"
        };

        //Act
        var command = new UserRegisterationCommand(user);
        var handler = new UserRegisterationCommandHandler(parameters, userRepo);

        try
        {    
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.UserAlreadyExists, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Test aborted due to exception: {ex.Message}");
        }

    }


    [Fact]
    public async Task Handler_ShouldReturnFalse_WhenEmailIsNotUnique_LoginTest()
    {
        //Arrange
        var LoginDTO = new UserLoginDTO()
        {
            Email = "Test@gmail.com",
            Password = "P@ssw0rd123",
            Username = "JohnDoe"
        };
        //Act
        var userRepo = new GenericRepository<User>(_dbContext);
        var jwtService = new JWTService(_mockConfiguration);
        var parameters = new BaseRequestParameters(
            new Mock<UserNameHasher>().Object,
            new Mock<PasswordHasher>().Object,
            jwtService
        );
      
        var command = new UserLoginCommand(LoginDTO);
        var CommandHandler = new UserLoginCommandHandler(parameters, userRepo);
        var result = await CommandHandler.Handle(command, CancellationToken.None);
        //Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]

    public async Task Handler_ShouldReturnFailure_WhenContentIsNull_AddPostTest()
    {
        //Arrange
         var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new BlogDbContext(options);
        var postRepo = new GenericRepository<Post>(_dbContext);
        var jwtService = new JWTService(_mockConfiguration);
        var parameters = new BaseRequestParameters(
            new Mock<UserNameHasher>().Object,
            new Mock<PasswordHasher>().Object,
            jwtService
        );
        var AddPostDTO = new AddPostDto()
        {
            Title = "Test Post",
            Username = "JohnDoe",
            Content = null, // Invalid content
            CreatedAt = DateTime.UtcNow,
        };

        //Act
        var command = new AddPostCommand(AddPostDTO);   
        var CommandHandler = new AddPostCommandHandler(parameters, postRepo);
        var result = await CommandHandler.Handle(command, CancellationToken.None);
        //Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(ErrorCode.DatabaseError);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidPostData_WhenUpdatePostWithInvalidData()
    {
        // Arrange
      
        var postRepo = new GenericRepository<Post>(_dbContext);
        var jwtService = new JWTService(_mockConfiguration);
        var parameters = new BaseRequestParameters(
            new Mock<UserNameHasher>().Object,new Mock<PasswordHasher>().Object,jwtService);
        var updatePostDto = new UpdatePostDto
        {
            ID = Guid.Empty,           // Invalid ID
            Title = "Updated Title",
            Content = "Updated Content"
        };
        var command = new UpdatePostByIdCommand(updatePostDto);
        var handler = new UpdatePostByIdCommandHandler(parameters, postRepo);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(ErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenUserIdDoesNotExist_GetRoleByID()
    {
        // Arrange
      
        var mockUserRepo = new Mock<GenericRepository<User>>(_dbContext);
        var mockParameters = new Mock<BaseRequestParameters>();
        // Simulate user not found
        mockUserRepo
      .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
      .ReturnsAsync((User?)null); // Simulate not found


        var query = new GetUserRoleByIDQuery("NonExistentUserID");
        var handler = new GetUserRoleByIDQueryHandler(mockParameters.Object, mockUserRepo.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(ErrorCode.UserNotFound);
    }

}
