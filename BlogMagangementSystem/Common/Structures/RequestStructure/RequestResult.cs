using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.Helpers;

namespace BlogMagangementSystem.Common.Structures.RequestStructure
{
    public record RequestResult<T>(T Data, bool IsSuccess, string Message, ErrorCode ErrorCode)
    {
        public static RequestResult<T> Success(T data, string message = "")
        {
            return new RequestResult<T>(data, true, message, ErrorCode.None);
        }

        public static RequestResult<T> Failure(ErrorCode errorCode)
        {
            return new RequestResult<T>(default!, false, errorCode.GetDescription(), errorCode);
        }
    }
}
