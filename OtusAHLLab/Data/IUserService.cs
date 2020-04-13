using System.Collections.Generic;
using System.Threading.Tasks;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetAllUserAsync();
    }
}