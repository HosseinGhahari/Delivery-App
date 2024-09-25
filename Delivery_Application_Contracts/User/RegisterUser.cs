using System.ComponentModel.DataAnnotations;

namespace Delivery_Application_Contracts.User
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="لطفا نام کاربری را وارد کنید")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
        public string Password { get; set; }
        [Required(ErrorMessage = "لطفا تکرار رمز عبور را وارد کنید")]
        public string ConfirmPassword { get; set; }
    }
}
