using Delivery_App.UtilityClass;
using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.AuthAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages
{
    // BasePageModel serves as a base class for all pages in our application.
    // Its primary role is to manage the display of both Paid and NotPaid prices.
    // By having all pages inherit from this base class, we ensure consistent
    // handling and display of these prices across the application.
    public class BasePageModel : PageModel
    {

        // ViewData attribute is used to pass data from controllers to views.
        // PaidPrice and NotPaidPrice are properties that hold the prices
        // for paid and unpaid items respectively.

        [ViewData]
        public string PaidPrice { get; set; }
        [ViewData]
        public string NotPaidPrice { get; set; }

        public string UserName { get; set; }

        // This is a constructor that takes an IDeliveryApplication object as a parameter.
        // It initializes the _deliveryApplication field and sets the PaidPrice and NotPaidPrice properties.
        // It also sets the static PriceHolder's PaidPrice and NotPaidPrice properties.
        // Also to show the prices in pages we convert it to string for better display format

        private readonly IDeliveryApplication _deliveryApplication;
        private readonly UserManager<User> _userManager;
        public BasePageModel(IDeliveryApplication deliveryApplication, UserManager<User> userManager)
        {
            _deliveryApplication = deliveryApplication;
            _userManager = userManager;
        }

        // The parameterless constructor is used to initialize PaidPrice and NotPaidPrice
        // with the values from the static PriceHolder class. This is necessary because
        // other pages that inherit from BasePageModel may use this constructor, and we
        // need to ensure that the prices are set even when the _deliveryApplication
        // dependency is not provided.
        public BasePageModel()
        {
            PaidPrice = PriceHolder.PaidPrice;
            NotPaidPrice = PriceHolder.NotPaidPrice;
        }

        // OnGetPricesAsync fetches paid and unpaid prices using _deliveryApplication,
        // converts them to string for display, and updates the static PriceHolder for shared access.
        public async Task OnGetPricesAsync()
        {
            double PPrice = await _deliveryApplication.GetPaidPriceAsync();
            PaidPrice = Convert.ToString(PPrice.Toman());

            double NPPrice = await _deliveryApplication.GetNotPaidPriceAsync();
            NotPaidPrice = Convert.ToString(NPPrice.Toman());

            PriceHolder.PaidPrice = PaidPrice;
            PriceHolder.NotPaidPrice = NotPaidPrice;

        }


        // OnGetUserNameAsync checks if the user is authenticated,
        // fetches the username using _userManager, and stores
        // it in both UserName and ViewData for display.
        public async Task OnGetUserNameAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToPage("/Account/Welcome");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                UserName = currentUser.UserName;
                ViewData["UserName"] = UserName;
            }
        }

    }
}
