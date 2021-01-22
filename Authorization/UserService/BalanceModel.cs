using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public class BalanceModel
    {
        [Required(ErrorMessage = "Top Up amount is required")]
        public decimal Balance { get; set; }
    }
}
