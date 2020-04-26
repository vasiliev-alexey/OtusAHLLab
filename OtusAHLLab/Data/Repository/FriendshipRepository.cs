using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using OtusAHLLab.Data.Interfaces;
using OtusAHLLab.Modules.Domain;
using OtusAHLLab.Modules.Enums;

namespace OtusAHLLab.Data.Repository
{
    public class FriendshipRepository : BaseRepository, IFriendshipRepository
    {
        public FriendshipRepository(string conn) : base(conn, Enumerable.Empty<string>())
        {
        }

        public async Task<bool> RemoveFriendShip(Friendship friendship)
        {
            using var db = GetDbConnection();
            var delRow = await db.ExecuteAsync("DELETE FROM AspNetUsers WHERE user_one_id = @UserId  " +
                                               " and user_two_id = @FriendId", friendship);

            return await Task.FromResult(delRow > 0);
        }

        public Task<bool> AddFriendship(long currUserId, long friendID)
        {
            using var connection = GetDbConnection();
            var rowIns = connection.ExecuteAsync("insert into friendship (user_one_id, user_two_id, action_user_id) " +
                                                 "values (@UserId,@FriendId,@ActionUserId)",
                new {UserId = currUserId, FriendId = friendID, ActionUserId = currUserId});
            return   Task.FromResult(rowIns.Result > 0);
        }

        public Task<bool> UpdateFriendship(long currAppuserId, long friendId, StatusCode accepted)
        {
            using (var db = GetDbConnection())
            {
                var updRowCnt =  db.Execute(@"UPDATE friendship
                SET status = @accepted,
                    action_user_id = @FriendId
                WHERE user_one_id = @UserId 
                and user_two_id=@FriendId ", new { UserId = currAppuserId , FriendId = friendId, Accepted= accepted });
                return   Task.FromResult(updRowCnt > 0);
            }
        }

      

        public Task<bool> RemoveFriendship(  long currAppuserId,   int friendId)
        {
            using var db = GetDbConnection();
            var delRow =  db.Execute("DELETE FROM friendship WHERE user_one_id = @UserId  " +
      
                                     " and user_two_id = @FriendId", new { UserId = currAppuserId, FriendId = friendId } );

            return  Task.FromResult(delRow > 0);
        }

        public async Task<bool> UpdateFriendship(Friendship friendship)
        {
            using (var db = GetDbConnection())
            {
                var updRowCnt = await db.ExecuteAsync(@"UPDATE friendship
                SET status = @UserName,
                    action_user_id = @ActionUserId
                WHERE user_one_id = @UserId 
                and user_two_id=@FriendId ", friendship);
                return await Task.FromResult(updRowCnt > 0);
            }
        }
    }
}