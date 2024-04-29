using Delivery_Application_Contracts.Destination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    // here we have a list of DestinationViewModel objects
    // and also An instance of the IDestinationApplication interface
    // and the OnGet method is called when the page is accessed
    // It retrieves all destinations and assigns them to the Destinations list
    // and show them on index view to user
    public class IndexModel : PageModel
    {
        public List<DestinationViewModel> Destinations;

        private readonly IDestinationApplication _destinationApplication;
        public IndexModel(IDestinationApplication destinationApplication)
        {
            _destinationApplication = destinationApplication;
        }
        public void OnGet()
        {
            Destinations = _destinationApplication.GetAll();
        }
    }
}
