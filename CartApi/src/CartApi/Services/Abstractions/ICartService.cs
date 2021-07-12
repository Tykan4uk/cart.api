using System.Threading.Tasks;
using CartApi.Models.Responses;

namespace CartApi.Services.Abstractions
{
    public interface ICartService
    {
        Task<AddCartResponse> AddAsync(int userId, int gameId);
        Task<GetCartResponse> GetAsync(int userId);
        Task<RemoveCartResponse> RemoveAsync(int userId, int gameId);
    }
}