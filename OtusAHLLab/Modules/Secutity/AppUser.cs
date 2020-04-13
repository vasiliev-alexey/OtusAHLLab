using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OtusAHLLab.Modules.Secutity
{
    public class AppUser : IdentityUser<int>
    {
        [PersonalData]
        [Required]
        [StringLength(50)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        [StringLength(50)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }


        [PersonalData]
        [Required]
        [Display(Name = "Возраст")]
        public uint Age { get; set; }

        [PersonalData]
        [Required]
        [Display(Name = "Пол")]
        public Gender Gender { get; set; }

        [PersonalData]
        [Required]
        [StringLength(100)]
        [Display(Name = "Город")]
        public string City { get; set; }

        [PersonalData]
        [Required]
        [StringLength(1000)]
        [Display(Name = "Увлечения")]
        public string Hobby { get; set; }
    }
}