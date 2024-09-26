using Delivery_Application_Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Auth
{
    // This class represents the registration page model, binding the RegisterUser DTO 
    // for user registration. It injects the IUserApplication to access user-related 
    // operations, such as registration.
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterUser RegisterUser { get; set; }

        private readonly IUserApplication _userApplication;
        public RegisterModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        // This method handles GET requests for the registration page. 
        // If the user is already authenticated, it redirects to the Index page; 
        // otherwise, it returns the registration page.
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        // This asynchronous method handles POST requests for user registration. 
        // It checks the model state for validity and attempts to register the user 
        // using the RegisterUser DTO. If successful, it redirects to the login page; 
        // otherwise, it stores the error message in TempData and returns the registration page.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            var result = await _userApplication.RegisterAsync(RegisterUser);
            if (result.IsSucceeded)
                return RedirectToPage("/Account/Login");
            else
                TempData["ErrorMessage"] = result.Message;

            return Page();
        }
    }
}
