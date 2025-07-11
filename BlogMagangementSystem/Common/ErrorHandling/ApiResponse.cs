namespace BlogMagangementSystem.Common.ErrorHandling
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Massege { get; set; }

        public ApiResponse(int stauscode, string? massege = null)
        {
            StatusCode = stauscode;
            Massege = massege ?? GetDefaultMessageForStatusCode(stauscode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {

                200 => "Ok",
                204 => "No Content",
                400 => "BadRequest",
                401 => "Unauthorized",
                404 => "Resources Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
