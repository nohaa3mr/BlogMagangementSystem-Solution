using DotNetCore.CAP;
using MessageBrokerService.UseCases.Events;

namespace MessageBrokerService.UseCases.Subscribers
{
    /// <summary>
    /// Use Case 1: Sending Notifications (Email, SMS, Push)
    /// This subscriber handles all notification-related events
    /// </summary>
    public class NotificationSubscriber : ICapSubscribe
    {
        private readonly ILogger<NotificationSubscriber> _logger;

        public NotificationSubscriber(ILogger<NotificationSubscriber> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Send welcome email when user registers
        /// </summary>
        [CapSubscribe("UserRegisteredEvent")]
        public async Task HandleUserRegistered(UserRegisteredEvent @event)
        {
            _logger.LogInformation("Sending welcome email to: {Email}", @event.Email);
            
            // Simulate email sending
            await Task.Delay(100); // Simulate async email service call
            
            // In real application:
            // await _emailService.SendWelcomeEmail(@event.Email, @event.FullName);
            
            _logger.LogInformation("Welcome email sent to: {Email}", @event.Email);
        }

        /// <summary>
        /// Send verification confirmation email
        /// </summary>
        [CapSubscribe("UserEmailVerifiedEvent")]
        public async Task HandleEmailVerified(UserEmailVerifiedEvent @event)
        {
            _logger.LogInformation("Sending verification confirmation email to: {Email}", @event.Email);
            
            await Task.Delay(100);
            
            // In real application:
            // await _emailService.SendVerificationConfirmation(@event.Email);
            
            _logger.LogInformation("Verification confirmation sent to: {Email}", @event.Email);
        }

        /// <summary>
        /// Notify author when their blog post is published
        /// </summary>
        [CapSubscribe("BlogPostPublishedEvent")]
        public async Task HandlePostPublished(BlogPostPublishedEvent @event)
        {
            _logger.LogInformation("Sending publication notification to author: {AuthorEmail}", @event.AuthorEmail);
            
            await Task.Delay(100);
            
            // In real application:
            // await _emailService.SendPostPublishedNotification(
            //     @event.AuthorEmail, 
            //     @event.Title, 
            //     @event.Slug);
            
            _logger.LogInformation("Publication notification sent to: {AuthorEmail}", @event.AuthorEmail);
        }

        /// <summary>
        /// Notify post author when someone comments on their post
        /// </summary>
        [CapSubscribe("CommentAddedEvent")]
        public async Task HandleCommentAdded(CommentAddedEvent @event)
        {
            _logger.LogInformation("New comment added to post: {PostId} by: {AuthorName}", 
                @event.PostId, @event.AuthorName);
            
            // Only notify if it's not the author commenting on their own post
            // In real app, fetch post author and compare
            
            await Task.Delay(100);
            
            // In real application:
            // var postAuthor = await _postRepository.GetAuthorAsync(@event.PostId);
            // if (postAuthor.Id != @event.AuthorId)
            // {
            //     await _emailService.SendCommentNotification(
            //         postAuthor.Email, 
            //         @event.AuthorName, 
            //         @event.Content);
            // }
            
            _logger.LogInformation("Comment notification processed for post: {PostId}", @event.PostId);
        }
    }
}

