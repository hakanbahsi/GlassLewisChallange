namespace GlassLewisChallange.Infrastructure.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ApiResponse<T> { Data = data, Success = true, Message = message };
        }

        public static ApiResponse<T> FailResponse(List<string> errors, string? message = null)
        {
            return new ApiResponse<T> { Success = false, Errors = errors, Message = message };
        }
    }
}
