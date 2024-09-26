using System.ComponentModel.DataAnnotations;

namespace Delivery_Application_Contracts.User
{
    // This class represents the model for user registration, containing 
    // fields for 'UserName', 'Password', and 'ConfirmPassword'. Each property 
    // is decorated with validation attributes to ensure they are required, 
    // providing custom error messages in Persian when validation fails.
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
