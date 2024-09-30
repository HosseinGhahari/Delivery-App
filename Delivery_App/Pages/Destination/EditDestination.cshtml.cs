using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.AuthAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    public class EditDestinationModel : BasePageModel
    {
        // This is the command object that will be used to edit a destination.
        public EditDestination Command;


        // here we inject our destinationapplication and we inherit the 
        // constructor from the base() because we don't have any dependency
        // for destination in our basePageModel so we use parameterless
        // constructor to take data (PaidPrice & NotPaidPrice) from
        // that basepage and display it

        private readonly IDestinationApplication _destinationApplication;
        private readonly UserManager<User> _userManager;

        public EditDestinationModel(IDestinationApplication destinationApplication,
            IDeliveryApplication deliveryApplication, UserManager<User> userManager) : base(deliveryApplication,userManager)
        {
            _destinationApplication = destinationApplication;
            _userManager = userManager;
        }

        // Fetches destination details for editing based on the provided ID.
        // Redirects to a NotFound page if the destination does not exist.
        // Retrieves the current user's name and delivery prices using base class methods.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Command = await _destinationApplication.GetEditDetailsAsync(id);
            if (Command == null)
            {
                return RedirectToPage("./NotFound"); 
            }

            await base.OnGetUserNameAsync();
            await base.OnGetPricesAsync();
            return Page();
        }


        // This post method takes an EditDestination command object as
        // a parameter, which contains the data from the form submission.
        // It calls the Edit method on the destination application service
        // to update the destination in the data store. After updating
        // the destination, it redirects to the Index page.
        public async Task<IActionResult> OnPostAsync(EditDestination command)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                TempData["Error"] = "کاربر احراز هویت نشده است.";
                return RedirectToPage("/Account/Login");
            }
            command.UserId = userId;
            ModelState.Remove("UserId");

            if(await _destinationApplication.ExistAsync(command.DestinationName, command.Id))
            {
                TempData["Exist"] = "مقصد مورد نظر تکراری میباشد";
                return Page();
            }

            if (ModelState.IsValid)
            {
                await _destinationApplication.EditAsync(command);
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
