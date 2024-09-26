using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Auth
{
    // This class represents the welcome page model. 
    // The OnGet method checks if the user is authenticated; 
    // if so, it redirects to the Index page. Otherwise,
    // it returns the welcome page
    public class WelcomeModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
