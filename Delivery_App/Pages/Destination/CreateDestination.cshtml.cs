using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.AuthAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    public class CreateDestinationModel : BasePageModel
    {
        // This is the command object that will be used to create a new destination.
        public CreateDestination command { get; set; }
        

        // here we inject our destinationapplication and we inherit the 
        // constructor from the base() because we don't have any dependency
        // for destination in our basePageModel so we use parameterless
        // constructor to take data (PaidPrice & NotPaidPrice) from
        // that basepage and display it

        private readonly IDestinationApplication _destinationApplication;
        private readonly UserManager<User> _userManager;
        public CreateDestinationModel(IDestinationApplication destinationApplication,
            IDeliveryApplication deliveryApplication, UserManager<User> userManager) : base(deliveryApplication,userManager)
        {
            _destinationApplication = destinationApplication;
            _userManager = userManager;
        }

        // Calls base methods to retrieve the current
        // user's name and delivery prices on page load.
        public async Task OnGet()
        {
            await base.OnGetUserNameAsync();
            await base.OnGetPricesAsync();
        }


        // this post method It takes a CreateDestination command object
        // as a parameter, which contains the data from the form submission.

        // also hecks if a destination with the same name already exists.
        // If it does, it sets a TempData message and redirects back to
        // the CreateDestination page.

        // checks if the ModelState is valid, meaning all form data
        // is valid according to validation rules.
        // If it is, it calls the Create method on the destination
        // application service and redirects to the Index page.

        public async Task<IActionResult> OnPostAsync(CreateDestination command)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                TempData["Error"] = "کاربر احراز هویت نشده است.";
                return RedirectToPage("/Account/Login");
            }
            command.UserId = userId;
            ModelState.Remove("UserId");

            if (await _destinationApplication.ExistAsync(command.DestinationName,null))
            {
                TempData["Exist"] = "مقصد مورد نظر تکراری میباشد";
                return RedirectToPage();
            }

            if (ModelState.IsValid)
            {         
                await _destinationApplication.CreateAsync(command);
                return RedirectToPage("./Index");
            }

            TempData["Error"] = "لطفا مقادیر خواسته شده را به صورت صحیح تکمیل نمایید";
            return RedirectToPage();
        }
    }
}
