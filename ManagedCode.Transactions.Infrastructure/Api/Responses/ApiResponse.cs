using Newtonsoft.Json;

namespace ManagedCode.Transactions.Infrastructure.Api.Responses;

public class ApiResponse<T>
{
    [JsonProperty("status")]
    public int StatusCode { get; set; }
    
    [JsonProperty("success")]
    public bool Success { get; set; }
    
    [JsonProperty("message")]
    public string Message { get; set; }
    
    [JsonProperty("data")]
    public T? Data { get; set; }
    
    [JsonProperty("errors")]
    public Dictionary<string, List<string>>? Errors { get; set; }

    // Success response constructor
    public ApiResponse(T data, string message = "Request successful")
    {
        StatusCode = 200;
        Success = true;
        Message = message;
        Data = data;
        Errors = null;
    }

    // Error response constructor
    public ApiResponse(int statusCode, string message, Dictionary<string, List<string>>? errors = null)
    {
        StatusCode = statusCode;
        Success = false;
        Message = message;
        Data = default;
        Errors = errors;
    }
}
