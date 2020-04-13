using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtusAHLLab.Data.Interfaces;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<AppUser>> GetAllUserAsync()
        {
            var rng = new Random();
            return Task.FromResult(_repository.GetUsers());
        }

    }
}
