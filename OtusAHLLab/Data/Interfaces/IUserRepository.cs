using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtusAHLLab.Modules.Enums;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data.Interfaces
{
    public interface IUserRepository
    {
        void Delete(int id);
        AppUser Get(int id);
        IEnumerable<AppUser> GetUsers(StatusCode statusCode);
        void Update(AppUser user);
        IEnumerable<AppUser> GetCandidateUsers(long currentUser);

        IEnumerable<AppUser> GetReqFriendShips(long currentUser);
        IEnumerable<AppUser> GetFriends(long curAppuserId);
        IEnumerable<AppUser> GetIncomeReqFriendShips(long curAppUserId);
    }
}