using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages
{
    public class IndexModel : BasePageModel
    {

        // here we easily Injecting DeliveryApplication interface
        // to fetch all database records and display them in the view.

        public List<DeliveryViewModel> Deliveries { get; set; }
        private readonly IDeliveryApplication _deliveryApplication;


        // The reason that we inherit from base(deliveryApplication) is to
        // ensure that the BasePageModel’s properties and methods, including
        // the initialization of PaidPrice and NotPaidPrice, are available
        // and properly set up in IndexModel.

        public IndexModel(IDeliveryApplication deliveryApplication ) : base( deliveryApplication )
        {
            _deliveryApplication = deliveryApplication;
        }

        // This Method store all the Deliveries
        public void OnGet()
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