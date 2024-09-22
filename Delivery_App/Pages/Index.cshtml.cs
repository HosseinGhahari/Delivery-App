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

        // This 'Search' property is used to hold the search term provided
        // by the user for search operations in the application. 
        // The 'SupportsGet' attribute indicates that this property
        // can also be populated from query string values in a HTTP GET request.
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }


        // The reason that we inherit from base(deliveryApplication) is to
        // ensure that the BasePageModel’s properties and methods, including
        // the initialization of PaidPrice and NotPaidPrice, are available
        // and properly set up in IndexModel.

        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IUserApplication _userApplication;
        private readonly UserManager<User> _userManager;
        public IndexModel(IDeliveryApplication deliveryApplication, IUserApplication userApplication,
            UserManager<User> userManager) : base(deliveryApplication)
        {
            _deliveryApplication = deliveryApplication;
            _userApplication = userApplication;
            _userManager = userManager;
        }


        // The OnGet method Check if the user is authenticated
        // manages pagination for delivery records. It calculates
        // the total count, determines the records for the current
        // page, and assigns them to the Deliveries property.
        public async Task<IActionResult> OnGetAsync(int p = 1, int s = 20 )
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Welcome");
            }

            var userId = _userManager.GetUserId(User);
            if(string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            PageSize = s;
            CurrentPage = p;

            if (string.IsNullOrEmpty(Search))
            {
                Search = string.Empty;
            }

            var allDeliveries = await _deliveryApplication.SearchAsync(Search , userId);
            TotalDeliveries = allDeliveries.Count;

            int skipCount = (CurrentPage - 1) * PageSize;
            Deliveries = allDeliveries.Skip(skipCount).Take(PageSize).OrderByDescending(x => x.Id).ToList();

            await InitializePricesAsync();
            ViewData["Search"] = Search;

            return Page();
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