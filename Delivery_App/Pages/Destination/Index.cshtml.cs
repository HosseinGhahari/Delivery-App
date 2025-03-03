using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.AuthAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    // here we have a list of DestinationViewModel objects
    // and also An instance of the IDestinationApplication interface
    // and the OnGet method is called when the page is accessed
    // It retrieves all destinations and assigns them to the Destinations list
    // and show them on index view to user , also is able to do
    // a search base on destinations name 
    public class IndexModel : BasePageModel
    {
        public List<DestinationViewModel> Destinations;

        [BindProperty(SupportsGet = true)]
        public string DestinationSearch { get; set; }

        private readonly IDestinationApplication _destinationApplication;
        private readonly UserManager<User> _userManager;
        public IndexModel(IDestinationApplication destinationApplication, IDeliveryApplication deliveryApplication,
            UserManager<User> userManager) : base(deliveryApplication,userManager)
        {
            _destinationApplication = destinationApplication;
            _userManager = userManager;
        }
        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
               Unauthorized();
  
            Destinations = await _destinationApplication.DestinationSearch(DestinationSearch,userId);
            ViewData["DestinationSearch"] = DestinationSearch;
            ViewData["SearchType"] = "Destinations";

            await base.OnGetUserNameAsync();
            await base.OnGetPricesAsync();
        }
    }
}
