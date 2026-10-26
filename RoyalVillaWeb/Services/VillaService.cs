using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using static RoyalVillaWeb.Models.SD;

namespace RoyalVillaWeb.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _villaUrl;
        private const string APIEndpoint = "/api/villa";
        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _clientFactory = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI"); ;
        }
        public Task<T?> CreateAsync<T>(CreateVillaDto dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = dto,
                Url = $"{_villaUrl}{APIEndpoint}",
                Token = token
            });
        }

        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.DELETE,
                Url = $"{_villaUrl}{APIEndpoint}/{id}",
                Token = token
            });
        }

        public Task<T?> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = $"{_villaUrl}{APIEndpoint}",
                Token = token
            });
        }

        public Task<T?> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = $"{_villaUrl}{APIEndpoint}/{id}",
                Token = token
            });
        }

        public Task<T?> UpdateAsync<T>(UpdateVillaDto dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.PUT,
                Data = dto,
                Url = $"{_villaUrl}{APIEndpoint}/{dto.Id}",
                Token = token
            });
        }
    }
}
