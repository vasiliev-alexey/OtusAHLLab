using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtusAHLLab.Data.Interfaces;
using OtusAHLLab.Modules.Enums;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IFriendshipRepository _friendshipRepository;

        public UserService(IUserRepository repository , IFriendshipRepository friendshipRepository)
        {
            _repository = repository;
            _friendshipRepository = friendshipRepository;
        }

        public Task<IEnumerable<AppUser>> GetAllUserAsync(StatusCode statusCode)
        {
            var rng = new Random();
            return Task.FromResult(_repository.GetUsers(statusCode));
        }
        public Task<IEnumerable<AppUser>> GetAvialibleUserAsync(long currAppuserId)
        {
        
            return Task.FromResult(_repository.GetCandidateUsers(currAppuserId)) ;
        }

        public Task<IEnumerable<AppUser>> GetFriendsAsync(long currAppuserId)
        {
            return Task.FromResult(_repository.GetFriends(currAppuserId));
        }

        public Task<IEnumerable<AppUser>> GetRequestedFriendships(long currAppuserId)
        {

            return Task.FromResult(_repository.GetReqFriendShips(currAppuserId));
        }

        public async Task<IEnumerable<AppUser>> GetCandidatesByNames(string firstNamePattern, string lastNamePatterns)
        {
            var users = await _repository.GetCandidatesByNames(firstNamePattern, lastNamePatterns);

            return users;
        }

        public Task<IEnumerable<AppUser>> GetIncomeFriendships(long curAppUserId)
        {
            return Task.FromResult(_repository.GetIncomeReqFriendShips(curAppUserId));
        }


        public Task<bool> RequestFriendship(long currAppuserId, long friendId)
        {
           return _friendshipRepository.AddFriendship(currAppuserId, friendId);
        }

        public Task<bool> ConfirmFriendship(long currAppuserId, long friendId)
        {
            return _friendshipRepository.UpdateFriendship(currAppuserId, friendId, StatusCode.Accepted);
        }

        public Task<bool> RejectFriendship(long currAppuserId, long friendId)
        {
            return _friendshipRepository.UpdateFriendship(currAppuserId, friendId, StatusCode.Declined);
        }

        public Task<bool> RemoveFriendship(long currAppuserId, int friendId)
        {
            return _friendshipRepository.RemoveFriendship(currAppuserId, friendId);
        }
    }
}
