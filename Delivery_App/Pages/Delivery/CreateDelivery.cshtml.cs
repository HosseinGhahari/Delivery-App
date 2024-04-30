using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Delivery_App.Pages.Delivery
{
    public class CreateDeliveryModel : PageModel
    {
        // These are public properties that will be used in the Razor view.

        public SelectList destinations;
        public DateTime date;

        // Checking Correct Format For Persian Date
        Regex regex = new Regex(@"^\d{4}/\d{1,2}/\d{1,2}$");

        [BindProperty]
        public string persiantime { get; set; }

        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IDestinationApplication _destinationApplication;
        private readonly IDeliveryRepository _deliveryRepository;
        public CreateDeliveryModel(IDeliveryApplication deliveryApplication 
            ,IDestinationApplication destinationApplication
            ,IDeliveryRepository deliveryRepository)
        {
            _deliveryApplication = deliveryApplication;
            _destinationApplication = destinationApplication;
            _deliveryRepository = deliveryRepository;
        }


        // Get all destinations from the application layer and
        // transform each destination into a new anonymous object.
        // The anonymous object includes the destination's Id and
        // a description that combines the destination name and price.
        // The resulting collection of anonymous objects is used to
        // create a SelectList, which is often used for dropdown lists in views.
        // Also Convert the current date and time to a Persian date string
        public void OnGet() 
        {
            destinations = new SelectList(_destinationApplication.GetAll()
            .Select(x => new
            {
                x.Id,
                Description = x.DestinationName + " *** " + x.Price + " تومان"
                
            }), "Id", "Description");

            date = DateTime.Now;
            persiantime = _deliveryRepository.ToPersiandate(date);

        }

        // It takes a CreateDelivery object and a boolean
        // as parameters, which contain the data from the
        // form submission.It updates the CreateDelivery
        // object, checks if the DestinationId & Date Formata
        // is valid, and then calls the Create method on the delivery
        // application service. After creating the delivery,
        // it redirects to the index page
        public RedirectToPageResult OnPost(CreateDelivery createDelivery , bool ispaid = false) 
        {
            createDelivery.IsPaid = ispaid;

            if (regex.IsMatch(persiantime))
            {
                createDelivery.DeliveryTime = _deliveryRepository.toGregoriandate(persiantime);
            }
            else
            {
                TempData["DateError"] = "لطفا تاریخ  را به درستی وارد کنید";
                return RedirectToPage();
            }


            if (createDelivery.DestinationId == 0)
            {
                TempData["DestinationError"] = "لطفا مقصد خود را به درستی وارد کنید";
                return RedirectToPage();
            }

            _deliveryApplication.Create(createDelivery);
            return RedirectToPage("/Index");
        }

    }
}
