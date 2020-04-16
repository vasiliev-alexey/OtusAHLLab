using System.Collections.Generic;
using System.Threading.Tasks;
using OtusAHLLab.Modules.Enums;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetAllUserAsync(StatusCode statusCode);
        Task<IEnumerable<AppUser>> GetAvialibleUserAsync(long currAppuserId);
        Task<IEnumerable<AppUser>> GetFriendsAsync(long currAppuserId);
        Task<IEnumerable<AppUser>> GetRequestedFriendships(long currAppuserId);

        Task<IEnumerable<AppUser>> GetIncomeFriendships(long currAppuserId);

        Task<bool> RequestFriendship(long currAppuserId, long friendId);
        Task<bool> ConfirmFriendship(long currAppuserId, long friendId);


        Task<bool> RejectFriendship(long currAppuserId, long friendId);
        Task<bool> RemoveFriendship(long toInt64, int id);
    }
}