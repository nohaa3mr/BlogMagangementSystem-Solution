using System.Text;

namespace BlogMagangementSystem.Common.Helpers.MessageBroker
{
    public sealed record MessageBrokerCommandResponse(string Message, bool IsSuccess, string ErrorMessage = "") 
    {
        public static MessageBrokerCommandResponse Success(string message)
        {
            return new MessageBrokerCommandResponse(message, true , "Message Retrieved Successfully !");
        }
        public static MessageBrokerCommandResponse Failure(string errorMessage)
        {
            return new MessageBrokerCommandResponse(string.Empty, false, errorMessage);
        }
    }
}