using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.User
{
    // This class represents the model for user login, containing fields 
    // for 'UserName' and 'Password'. Both properties are required and 
    // include custom error messages in Persian to guide the user during validation.
    public class LoginUser
    {
        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
        public string Password { get; set; }
    }
}
