using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtusAHLLab.Data;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


        // GET api/values
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult<IEnumerable<AppUser>>> Get(string firstName, string lastName)
        {

            var users = await _userService.GetCandidatesByNames(firstName, lastName);

           return   Ok(users);
        }
    }
}
