using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.User;
using Delivery_Domain.AuthAgg;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.AccountManagement
{
    public class IndexModel : BasePageModel
    {
        // The constructor injects IUserApplication, IDeliveryApplication, and UserManager<User>,
        // passing the latter two to the BasePageModel for initializing delivery prices and user info.

        public UsersViewModel Users { get; set; }

        private readonly IUserApplication _userApplication;
        public IndexModel(IUserApplication userApplication, IDeliveryApplication deliveryApplication, UserManager<User> userManager)
            : base(deliveryApplication, userManager) 
        {
            _userApplication = userApplication;
        }

        // Retrieves users from the application layer and redirects
        // to the login page if no users are found. Also retrieves
        // the current user's name and delivery prices by invoking base class methods.
        public async Task<IActionResult> OnGet()
        {
            await base.OnGetUserNameAsync();
            await base.OnGetPricesAsync();

            Users = await _userApplication.GetUsersAsync();
            if (Users == null)
            {
                return RedirectToPage("/Account/Login"); 
            }

            return Page();
        }

        public async Task<IActionResult> OnGetRemoveUserAsync(string id)
        {
            await _userApplication.RemoveAsync(id);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToPage("/Account/Welcome");
        }
    }
}
