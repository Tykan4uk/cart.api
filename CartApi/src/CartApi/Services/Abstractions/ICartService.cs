using System.Threading.Tasks;
using CartApi.Common.Enums;
using CartApi.Models.Responses;

namespace CartApi.Services.Abstractions
{
    public interface ICartService
    {
        Task<AddResponse> AddAsync(string userId, string productId, string name, string description, decimal price, ProductTypeEnum productType);
        Task<GetResponse> GetAsync(string userId);
        Task<RemoveResponse> RemoveAsync(string userId, string productId);
    }
}