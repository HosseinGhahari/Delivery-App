using Delivery_Application_Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginUser LoginUser { get; set; }

        private readonly IUserApplication _userApplication;
        public LoginModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid) 
                return Page();

            var result = await _userApplication.LoginAsync(LoginUser);
            if (result.IsSucceeded)
                return RedirectToPage("/Index");
            else
                TempData["ErrorMessage"] = result.Message;

            return Page();         
        }
    }
}
