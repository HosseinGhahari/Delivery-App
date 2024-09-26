using Delivery_Application_Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Auth
{
    // This class represents the login page model, binding the LoginUser DTO 
    // for user authentication. It injects the IUserApplication to access 
    // user-related operations, such as logging in.
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginUser LoginUser { get; set; }

        private readonly IUserApplication _userApplication;
        public LoginModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        // This method handles GET requests for the login page. 
        // If the user is already authenticated, it redirects to the Index page; 
        // otherwise, it returns the login page.
        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        // This method handles POST requests for user login. 
        // It checks the model state for validity and attempts to log in the user 
        // using the LoginUser DTO. If successful, it redirects to the Index page; 
        // otherwise, it stores the error message in TempData and returns the login page.
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
