using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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


        // The reason that we inherit from base(deliveryApplication) is to
        // ensure that the BasePageModel’s properties and methods, including
        // the initialization of PaidPrice and NotPaidPrice, are available
        // and properly set up in IndexModel.

        private readonly IDeliveryApplication _deliveryApplication;
        public IndexModel(IDeliveryApplication deliveryApplication ) : base( deliveryApplication )
        {
            _deliveryApplication = deliveryApplication;
        }


        // The OnGet method manages pagination for delivery records.
        // It calculates the total count, determines the records for
        // the current page, and assigns them to the Deliveries property.
        public void OnGet(int p = 1 , int s = 20)
        {
            PageSize = s;
            CurrentPage = p;

            var allDeliveries = _deliveryApplication.GetAll();
            TotalDeliveries = allDeliveries.Count;

            int skipCount = (CurrentPage - 1) * PageSize;
            Deliveries = allDeliveries.Skip(skipCount).Take(PageSize).ToList();
        }

        // This method handles the request to remove a delivery
        // record by its ID. After removal, it redirects the user
        // to the Index page.
        public IActionResult OnGetRemove(int id)
        {
            _deliveryApplication.Remove(id);
            return RedirectToPage("./Index");
        }

        // POST action to mark unpaid deliveries as paid and redirect to Index.
        // this action handel the checkout opreation for client also this handler
        // is on layout page in shared folder
        public IActionResult OnPostMarkAllAsPaid()
        {
            _deliveryApplication.MarkAllAsPaid();
            return RedirectToPage("./Index");
        }
    }
}