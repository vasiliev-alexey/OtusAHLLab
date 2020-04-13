using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;


        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

      
        public class InputModel
        {
            [Required]
            [EmailAddress(ErrorMessage = "Некорректный адрес")]
            [Display(Name = "Почтовый ящик")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} должно быть более {2} и менее {1} символов длины.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите пароль")]
            [Compare("Password", ErrorMessage = "Пароль не совпадает с введнным.")]
            public string ConfirmPassword { get; set; }

            [StringLength(100, ErrorMessage = "{0} должно быть более {2} и менее {1} символов длины.",
                MinimumLength = 2)]
            [Required]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            [StringLength(100, ErrorMessage = "{0} должно быть более {2} и менее {1} символов длины.",
                MinimumLength = 2)]
            [Required]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }


            [Range(1, 110, ErrorMessage = "Недопустимый возраст")]
            [Required]
            [Display(Name = "Возраст")]
            public uint Age { get; set; }


            [Required] [Display(Name = "Пол")]
            public Gender Gender { get; set; }

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

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName,
                    Hobby = Input.Hobby, Age = Input.Age, City = Input.City, Gender = (Input.Gender)
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}