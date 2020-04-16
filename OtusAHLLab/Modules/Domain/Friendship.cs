using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Modules.Domain
{
    public class Friendship
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int FriendId { get; set; }
        public AppUser FriendUser { get; set; }


        public int ActionUserId { get; set; }
        public AppUser ActionUser { get; set; }

    }
}
