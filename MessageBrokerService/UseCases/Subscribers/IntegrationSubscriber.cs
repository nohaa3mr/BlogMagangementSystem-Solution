using DotNetCore.CAP;
using MessageBrokerService.UseCases.Events;

namespace MessageBrokerService.UseCases.Subscribers
{
    /// <summary>
    /// Use Case 4: Microservice Integration
    /// Integrate with external services or other microservices
    /// </summary>
    public class IntegrationSubscriber : ICapSubscribe
    {
        private readonly ILogger<IntegrationSubscriber> _logger;

        public IntegrationSubscriber(ILogger<IntegrationSubscriber> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sync user data to external CRM system
        /// </summary>
        [CapSubscribe("UserRegisteredEvent")]
        public async Task SyncUserToCRM(UserRegisteredEvent @event)
        {
            _logger.LogInformation("Syncing user to CRM: {UserId}", @event.UserId);
            
            // In real application:
            // await _crmService.CreateContact(new CRMContact
            // {
            //     Email = @event.Email,
            //     Name = @event.FullName,
            //     Source = "BlogPlatform"
            // });
            
            await Task.Delay(200); // Simulate external API call
            _logger.LogInformation("User synced to CRM: {UserId}", @event.UserId);
        }

        /// <summary>
        /// Send blog post to social media when published
        /// </summary>
        [CapSubscribe("BlogPostPublishedEvent")]
        public async Task ShareToSocialMedia(BlogPostPublishedEvent @event)
        {
            _logger.LogInformation("Sharing post to social media: {PostId}", @event.PostId);
            
            // In real application:
            // await _socialMediaService.PostToTwitter(new SocialMediaPost
            // {
            //     Title = @event.Title,
            //     Url = $"https://blog.example.com/posts/{@event.Slug}"
            // });
            
            // await _socialMediaService.PostToLinkedIn(...);
            // await _socialMediaService.PostToFacebook(...);
            
            await Task.Delay(300); // Simulate multiple API calls
            _logger.LogInformation("Post shared to social media: {PostId}", @event.PostId);
        }

        /// <summary>
        /// Update search index when post is published
        /// </summary>
        [CapSubscribe("BlogPostPublishedEvent")]
        public async Task UpdateSearchIndex(BlogPostPublishedEvent @event)
        {
            _logger.LogInformation("Updating search index for post: {PostId}", @event.PostId);
            
            // In real application:
            // var postContent = await _postRepository.GetFullContentAsync(@event.PostId);
            // await _searchService.IndexDocument(new SearchDocument
            // {
            //     Id = @event.PostId.ToString(),
            //     Title = @event.Title,
            //     Content = postContent,
            //     Slug = @event.Slug,
            //     PublishedDate = @event.DateTime
            // });
            
            await Task.Delay(150);
            _logger.LogInformation("Search index updated for post: {PostId}", @event.PostId);
        }

        /// <summary>
        /// Update user in email marketing platform when profile changes
        /// </summary>
        [CapSubscribe("UserProfileUpdatedEvent")]
        public async Task UpdateEmailMarketing(UserProfileUpdatedEvent @event)
        {
            _logger.LogInformation("Updating email marketing platform for user: {UserId}", @event.UserId);
            
            // In real application:
            // if (@event.ChangedFields.ContainsKey("Email"))
            // {
            //     await _emailMarketingService.UpdateContactEmail(
            //         @event.PreviousEmail, 
            //         @event.Email);
            // }
            // 
            // await _emailMarketingService.UpdateContactFields(@event.UserId, @event.ChangedFields);
            
            await Task.Delay(100);
            _logger.LogInformation("Email marketing platform updated for user: {UserId}", @event.UserId);
        }
    }
}

