using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages
{
    public class IndexModel : PageModel
    {


        // here we easily Injecting DeliveryApplication interface
        // to fetch all database records and display them in the view.

        public List<DeliveryViewModel> Deliveries { get; set; }
        private readonly IDeliveryApplication _deliveryApplication;

        public IndexModel(IDeliveryApplication deliveryApplication )
        {
            _deliveryApplication = deliveryApplication;
        }

        // This Method store all the Deliveries
        public void OnGet(DeliveryViewModel delivery)
        {           
            Deliveries = _deliveryApplication.GetAll();
        }

        // This method handles the request to remove a delivery
        // record by its ID. After removal, it redirects the user
        // to the Index page.
        public IActionResult OnGetRemove(int id)
        {
            _deliveryApplication.Remove(id);
            return RedirectToPage("./Index");
        }
    }
}