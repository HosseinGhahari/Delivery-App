using Delivery_Application_Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterUser RegisterUser { get; set; }

        private readonly IUserApplication _userApplication;
        public RegisterModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            var result = await _userApplication.RegisterAsync(RegisterUser);
            if (result.IsSucceeded)
                return RedirectToPage("/Auth/Login");
            else
                TempData["ErrorMessage"] = result.Message;

            return Page();
        }
    }
}
