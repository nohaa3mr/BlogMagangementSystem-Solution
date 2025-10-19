using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestForBlogSystem
{
    public class PostModuleTests
    {
        [Fact]
        public async Task Handle_Should_ReturnFailureResult_IfEmailIsNotUnique()
        {
            // Arrange 

            var command = new CreatePostCommand
            {
                Title = "Test Post",
                Content = "This is a test post content.",
                AuthorEmail = "
                //Act


                //Assert 

            }
    }
}

