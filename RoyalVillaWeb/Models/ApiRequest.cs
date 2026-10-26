using static RoyalVillaWeb.Models.SD;

namespace RoyalVillaWeb.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string? Url { get; set; }
        public object? Data { get; set; } // Maybe should be object type instead of string
        public string? Token { get; set; }
    }
}
