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
        // These are public properties that will be used in the Razor view.

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

        // This is the method that gets called when a GET
        // request is made to the CreateDelivery page.
        // It initializes the destinations SelectList and
        // the date and persiantime properties.
        public void OnGet() 
        {
            destinations = new SelectList(_destinationApplication.GetAll()
            .Select(x => new
            {
                x.Id,
                Description = x.DestinationName + " *** " + x.Price + " تومان"

            }), "Id", "Description");

            date = DateTime.Now;
            persiantime = _deliveryApplication.ToPersiandate(date);

        }

        // It takes a CreateDelivery object and a boolean
        // as parameters, which contain the data from the
        // form submission.It updates the CreateDelivery
        // object, checks if the DestinationId is valid,
        // and then calls the Create method on the delivery
        // application service. After creating the delivery,
        // it redirects to the 
        public RedirectToPageResult OnPost(CreateDelivery createDelivery , bool ispaid = false) 
        {
            createDelivery.IsPaid = ispaid;
            createDelivery.DeliveryTime = _deliveryApplication.toGregoriandate(persiantime);
            if(createDelivery.DestinationId == 0)
            {
                TempData["DestinationError"] = "لطفا مقصد خود را به درستی وارد کنید";
                return RedirectToPage();
            }

            _deliveryApplication.Create(createDelivery);
            return RedirectToPage("./Index");
        }

    }
}
