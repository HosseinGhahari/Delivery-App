using Delivery_Application_Contracts.Destination;
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
        public EditDestinationModel(IDestinationApplication destinationApplication) : base()
        {
            _destinationApplication = destinationApplication;
        }

        // This is the method that gets called when a GET request is
        // made to the EditDestination page. It takes an id as a parameter,
        // which is the id of the destination to be edited. It retrieves the
        // details of the destination to be edited and assigns it to the Command object.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Command = await _destinationApplication.GetEditDetailsAsync(id);
            if (Command == null)
            {
                return RedirectToPage("./NotFound"); 
            }
            return Page();
        }


        // This post method takes an EditDestination command object as
        // a parameter, which contains the data from the form submission.
        // It calls the Edit method on the destination application service
        // to update the destination in the data store. After updating
        // the destination, it redirects to the Index page.
        public async Task<IActionResult> OnPostAsync(EditDestination command)
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            await _destinationApplication.EditAsync(command);
            return RedirectToPage("./Index");
        }


    }
}
