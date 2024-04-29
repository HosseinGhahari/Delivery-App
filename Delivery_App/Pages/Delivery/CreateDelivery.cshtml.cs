using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Delivery_App.Pages.Delivery
{
    public class CreateDeliveryModel : PageModel
    {
        public SelectList destinations;
        public DateTime date;

        [BindProperty]
        public string persiantime { get; set; }

        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IDestinationApplication _destinationApplication;
        public CreateDeliveryModel(IDeliveryApplication deliveryApplication , IDestinationApplication destinationApplication)
        {
            _deliveryApplication = deliveryApplication;
            _destinationApplication = destinationApplication;
        }
        public void OnGet() 
        {
            destinations = new SelectList(_destinationApplication.GetAll(), "Id", "DestinationName");

            date = DateTime.Now;
            persiantime = _deliveryApplication.ToPersiandate(date);

        }
        public RedirectToPageResult OnPost(CreateDelivery createDelivery , bool ispaid = false) 
        {
            createDelivery.IsPaid = ispaid;
            createDelivery.DeliveryTime = _deliveryApplication.toGregoriandate(persiantime);
            _deliveryApplication.Create(createDelivery);
            return RedirectToPage();
        }

    }
}
