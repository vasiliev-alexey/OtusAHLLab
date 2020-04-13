using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OtusAHLLab.Modules.Secutity
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole()
        {
        }

        public AppRole(string name)
        {
            Name = name;
        }
    }
}