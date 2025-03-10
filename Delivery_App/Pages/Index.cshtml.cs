using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.User;
using Delivery_Domain.AuthAgg;
using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Delivery_App.Pages
{
    public class IndexModel : BasePageModel
    {
        // variables that hold needed amounts for pagination
        public int TotalDeliveries { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        // here we easily Injecting DeliveryApplication interface
        // to fetch all database records and display them in the view.
        public List<DeliveryViewModel> Deliveries { get; set; }
/*        // property to show username on index
        public string UserName { get; set; }*/


        // The reason that we inherit from base(deliveryApplication) is to
        // ensure that the BasePageModel’s properties and methods, including
        // the initialization of PaidPrice and NotPaidPrice, are available
        // and properly set up in IndexModel.

        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IUserApplication _userApplication;
        private readonly UserManager<User> _userManager;
        public IndexModel(IDeliveryApplication deliveryApplication, IUserApplication userApplication,
            UserManager<User> userManager) : base(deliveryApplication,userManager)
        {
            _deliveryApplication = deliveryApplication;
            _userApplication = userApplication;
            _userManager = userManager;
        }

        // This method handles the GET request for the page,
        // ensuring the user is authenticated. It retrieves the user's
        // ID, initializes pagination parameters,and fetches deliveries
        // based on the search criteria before returning the page.
        public async Task<IActionResult> OnGetAsync(int p = 1, int s = 20 )
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Welcome");
            }

            await base.OnGetUserNameAsync();

            var userId = _userManager.GetUserId(User);
            if(string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            PageSize = s;
            CurrentPage = p;

            var allDeliveries = await _deliveryApplication.GetDeliveries(userId);
            TotalDeliveries = allDeliveries.Count;

            int skipCount = (CurrentPage - 1) * PageSize;
            Deliveries = allDeliveries.Skip(skipCount).Take(PageSize).OrderByDescending(x => x.Id).ToList();

            await OnGetPricesAsync();
            ViewData["SearchType"] = "Deliveries";

            return Page();
        }

        // This API handles search requests sent via AJAX from the frontend.
        // It retrieves all deliveries for the logged-in user and filters them based
        // on the search query. The results are returned as a JSON response
        // to update the UI dynamically.

        public async Task<IActionResult> OnGetDeliverySearchAsync(string search)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var alldeliveries = await _deliveryApplication.GetDeliveries(userId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                alldeliveries = alldeliveries
                    .Where(d => d.Destination
                    .Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return new JsonResult(alldeliveries);
        }


        // This method handles the request to remove a delivery
        // record by its ID. After removal, it redirects the user
        // to the Index page.
        public async Task<IActionResult> OnGetRemoveAsync(int id)
        {
            await _deliveryApplication.RemoveAsync(id);
            return RedirectToPage("./Index");
        }

        // POST action to mark unpaid deliveries as paid and redirect to Index.
        // this action handel the checkout opreation for client also this handler
        // is on layout page in shared folder
        public async Task<IActionResult> OnPostMarkAllAsPaidAsync()
        {
            await _deliveryApplication.MarkAllAsPaidAsync();
            return RedirectToPage("./Index");
        }

        // This method handles the POST request for logging out the user.
        // It attempts to log out the user through the application layer and signs out 
        // the authentication cookie if successful, then redirects to the welcome page.
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var result = await _userApplication.LogOutAsync();
            if (result.IsSucceeded)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete(".AspNetCore.Identity.Application");
                return RedirectToPage("/Account/Welcome");
            }

            return RedirectToPage("/Error");
        }
    }
}