using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OtusAHLLab.Data;
using OtusAHLLab.Modules.Secutity;
using Serilog;

namespace OtusAHLLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;


        // GET api/values
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<AppUser>>> Get(string firstName, string lastName)
        {

            _logger.LogDebug($"find by fn=${firstName} and ln=${lastName}");

            var users = await _userService.GetCandidatesByNames(firstName, lastName);

           return   Ok(users);
        }
    }
}
