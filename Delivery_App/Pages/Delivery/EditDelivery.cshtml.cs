using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.AuthAgg;
using Delivery_Domain.DeliveryAgg;
using Delivery_Infrastructure.DateConversionService;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public EditDeliveryModel(IDeliveryApplication deliveryApplication
            , IDestinationApplication destinationApplication,
            IDeliveryRepository deliveryRepository
            , IDateConversionService dateConversionService,
            UserManager<User> userManager) : base(deliveryApplication,userManager)
        {
            _deliveryApplication = deliveryApplication;
            _destinationApplication = destinationApplication;
            _deliveryRepository = deliveryRepository;
            _deliveryConversionService = dateConversionService;
            _userManager = userManager;
        }


        // This method is called when the page is loaded. It retrieves the
        // data for the delivery that's being edited and initializes the form fields.
        // also we save the id of the command is stored in TempData,
        // which is a dictionary used for preserving data between controller actions.
        // also save the persian date in tempdata to have it displayed after redirect

        public async Task<IActionResult> OnGetAsync(int id)
        {
            command = await _deliveryApplication.GetEditDetailsAsync(id); // Make sure to implement async versions in the Application layer
            if (command == null)
            {
                return NotFound(); // Handle the case where the delivery is not found
            }

            TempData["CommandId"] = command.Id;

            var userid = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userid))
                Unauthorized();

            var destinationsData = await _destinationApplication.GetDestinationsAsync(userid); // Ensure this method is async in the Application layer
            destinations = new SelectList(destinationsData.Select(x => new
            {
                x.Id,
                Description = $"{x.DestinationName} *** {x.Price} تومان"
            }), "Id", "Description");

            if (command.DeliveryTime != null)
            {
                date = command.DeliveryTime;
                persiantime = _deliveryConversionService.ToPersiandate(date);
                TempData["OriginalPersianDate"] = persiantime;
            }
            else
            {
                persiantime = TempData["OriginalPersianDate"] as string;
                return RedirectToPage(new { id = TempData["CommandId"] });
            }

            await base.OnGetUserNameAsync();
            await base.OnGetPricesAsync();
            return Page(); // Return the page result
        }


        // This method is called when the form is submitted. It validates the
        // input, updates the delivery data, and redirects to the Index page.
        // also we pass the id that we saved to do the edit if validation failed 

        public async Task<IActionResult> OnPostAsync(EditDelivery command)
        {
            if (!regex.IsMatch(persiantime))
            {
                TempData["Datefailed"] = "لطفا تاریخ را به درستی وارد کنید";
                return RedirectToPage(new { id = command.Id });
            }

            command.DeliveryTime = _deliveryConversionService.toGregoriandate(persiantime);
            await _deliveryApplication.EditAsync(command); 
            return RedirectToPage("/Index");
        }
    }
}
