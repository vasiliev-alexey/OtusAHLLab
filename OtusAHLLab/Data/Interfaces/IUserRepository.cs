using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data.Interfaces
{
    public interface IUserRepository
    {
        void Delete(int id);
        AppUser Get(int id);
        IEnumerable<AppUser> GetUsers();
        void Update(AppUser user);
    }
}