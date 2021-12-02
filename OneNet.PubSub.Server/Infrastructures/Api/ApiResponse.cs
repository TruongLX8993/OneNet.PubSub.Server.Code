using Newtonsoft.Json;
using OneNet.PubSub.Server.Application.Exceptions;

namespace OneNet.PubSub.Server.Infrastructures.Api
{
    public class ApiResponse
    {
        private const int StatusOK = 0;
        private const int StatusError = -1;

        [JsonProperty("status")] public int Status { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("message")] public string Message { get; set; }

        public static ApiResponse CreateSuccess(object data)
        {
            return new ApiResponse()
            {
                Status = StatusOK,
                Data = data,
            };
        }

        public static ApiResponse CreateError(ApplicationException applicationException)
        {
            var res = new ApiResponse
            {
                Status = StatusError,
                Message = applicationException.Message
            };
            return res;
        }

        public static ApiResponse CreateServerInternalError()
        {
            var res = new ApiResponse
            {
                Status = StatusError,
                Message = "Server Internal Error"
            };
            return res;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}