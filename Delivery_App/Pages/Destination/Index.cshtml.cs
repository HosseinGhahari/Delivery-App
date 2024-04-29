using Delivery_Application_Contracts.Destination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
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
