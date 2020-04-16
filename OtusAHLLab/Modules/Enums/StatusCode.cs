using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OtusAHLLab.Modules.Enums
{
    public enum StatusCode
    {
        [Display(Prompt = "Запрошена")] Pending = 0,
        [Display(Prompt = "Принята")] Accepted = 1,
        [Display(Prompt = "Отклонена")] Declined = 2,
        [Display(Prompt = "Заблокирована")] Blocked = 3,
        [Display(Prompt = "Все")] All = 99
           

    }
}