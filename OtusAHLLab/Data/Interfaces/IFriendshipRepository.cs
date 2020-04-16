using System.Threading.Tasks;
using OtusAHLLab.Modules.Domain;
using OtusAHLLab.Modules.Enums;

namespace OtusAHLLab.Data.Interfaces
{
    public interface IFriendshipRepository
    {
        public Task<bool> RemoveFriendShip(Friendship friendship);
        public Task<bool> AddFriendship(long currUserId , long friendID);
      
        Task<bool> UpdateFriendship(long currAppuserId, long friendId, StatusCode accepted);
        Task<bool> RemoveFriendship(  long currAppuserId,   int friendId);
    }
}