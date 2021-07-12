using System.Threading.Tasks;
using CartApi.Data.Cache;

namespace CartApi.Services.Abstractions
{
    public interface ICacheService<TCacheEntity>
        where TCacheEntity : class, ICacheEntity
    {
        Task AddOrUpdateAsync(TCacheEntity entity);

        Task<TCacheEntity> GetAsync(int userId);
    }
}