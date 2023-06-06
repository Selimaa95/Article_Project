using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.VModels
{
    public class AccountVM
    {

        [Required(ErrorMessage ="برجاء ادخال اسم المستخدم او البريد الالكترونى")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "برجاء ادخال البريد الالكترونى")]
        [EmailAddress(ErrorMessage ="البريد الالكترونى غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage ="برجاء ادخال كلمه المرور")]
        public string Password { get; set; }

        [Required(ErrorMessage ="برجاء تاكيد كلمه المرور")]
        [Compare("Password",ErrorMessage ="كلمه المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        public string? Token { get; set; }
    }
}
