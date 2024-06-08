using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.DeliveryAgg;
using Delivery_Infrastructure.DateConversionService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Delivery_App.Pages.Delivery
{
    public class EditDeliveryModel : BasePageModel
    {   
        
        // This property is bound to the input field for the Persian date in the form.
        [BindProperty]
        public string persiantime { get; set; }

        // This object holds the data for the delivery that's being edited.
        public EditDelivery command;

        // This object is used to populate the dropdown list for destinations in the form.
        public SelectList destinations;

        // This object holds the delivery time as a DateTime.
        public DateTime date;

        // Checking Correct Format For Persian Date
        Regex regex = new Regex(@"^\d{4}/\d{1,2}/\d{1,2}$");


        // These fields hold references to the application services
        // and repositories that are used in this page. and constructor
        // initializes the application services and repositories.

        // The reason that we inherit from base(deliveryApplication) is to
        // ensure that the BasePageModel’s properties and methods, including
        // the initialization of PaidPrice and NotPaidPrice, are available
        // and properly set up in IndexModel.

        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IDestinationApplication _destinationApplication;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IDateConversionService _deliveryConversionService;

        public EditDeliveryModel(IDeliveryApplication deliveryApplication 
            , IDestinationApplication destinationApplication ,
            IDeliveryRepository deliveryRepository
            ,IDateConversionService dateConversionService) : base(deliveryApplication)
        {
            _deliveryApplication = deliveryApplication;
            _destinationApplication = destinationApplication;
            _deliveryRepository = deliveryRepository;
            _deliveryConversionService = dateConversionService;
        }


        // This method is called when the page is loaded. It retrieves the
        // data for the delivery that's being edited and initializes the form fields.
        // also we save the id of the command is stored in TempData,
        // which is a dictionary used for preserving data between controller actions.
        // also save the persian date in tempdata to have it displayed after redirect

        public void OnGet(int id)
        {
            command = _deliveryApplication.GetEditDetailes(id);
            TempData["CommandId"] = command.Id;

            destinations = new SelectList(_destinationApplication.GetAll().Select(x => new
            {
                x.Id,
                Description = x.DestinationName + " *** " + x.Price + " تومان"
            }), "Id", "Description");

            if (command != null && command.DeliveryTime != null)
            {
                date = command.DeliveryTime;
                persiantime = _deliveryConversionService.ToPersiandate(date);
                TempData["OriginalPersianDate"] = persiantime as string;
            }
            else
            {
                persiantime = TempData["OriginalPersianDate"] as string;
                RedirectToPage(new { id = TempData["CommandId"] });
            }

        }


        // This method is called when the form is submitted. It validates the
        // input, updates the delivery data, and redirects to the Index page.
        // also we pass the id that we saved to do the edit if validation failed 

        public RedirectToPageResult OnPost(EditDelivery command)
        {

            if (!regex.IsMatch(persiantime))
            {
                TempData["Datefailed"] = "لطفا تاریخ  را به درستی وارد کنید";
                return RedirectToPage(new { id = command.Id });
            }

            command.DeliveryTime = _deliveryConversionService.toGregoriandate(persiantime);
            _deliveryApplication.Edit(command);
            return RedirectToPage("/Index");

        }
    }
}
