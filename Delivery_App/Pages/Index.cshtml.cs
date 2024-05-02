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
        public void OnGet(DeliveryViewModel delivery)
        {
            
            Deliveries = _deliveryApplication.GetAll();
        }
    }
}