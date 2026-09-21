namespace RoyalVilla_API.Models.DTOs
{
    public class ApiResponse<TData>
    {
        public bool Success { set; get; }
        public int StatusCode { set; get; }
        public string? Message { set; get; }
        public TData? Data { set; get; }
        public object? Errors { set; get; }
        public DateTime Timestamp { set; get; } = DateTime.UtcNow;


        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }

        public static ApiResponse<TData> Ok(string message, TData? data) =>
            Create(true, StatusCodes.Status200OK, message, data);

        public static ApiResponse<TData> Created(string message, TData? data) =>
            Create(true, StatusCodes.Status201Created, message, data);

        public static ApiResponse<TData> NoContent(string message) =>
            Create(true, StatusCodes.Status204NoContent, message);

        public static ApiResponse<TData> NotFound(string message) =>
            Create(false, StatusCodes.Status404NotFound, message);

        public static ApiResponse<TData> BadRequest(string message, object? errors = null) =>
            Create(false, StatusCodes.Status400BadRequest, message, errors:errors);

        public static ApiResponse<TData> Conflict(string message, object? errors = null) =>
            Create(false, StatusCodes.Status409Conflict, message, errors: errors);

        public static ApiResponse<TData> Error(int statusCode, string message, object? errors = null) =>
            Create(false, statusCode, message, errors: errors);
    }
}
