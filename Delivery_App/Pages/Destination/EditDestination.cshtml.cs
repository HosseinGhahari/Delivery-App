using Delivery_Application_Contracts.Destination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    public class EditDestinationModel : PageModel
    {
        // This is the command object that will be used to edit a destination.
        public EditDestination Command;

        private readonly IDestinationApplication _destinationApplication;
        public EditDestinationModel(IDestinationApplication destinationApplication)
        {
            _destinationApplication = destinationApplication;
        }

        // This is the method that gets called when a GET request is
        // made to the EditDestination page. It takes an id as a parameter,
        // which is the id of the destination to be edited. It retrieves the
        // details of the destination to be edited and assigns it to the Command object.
        public void OnGet(int id)
        {
           Command = _destinationApplication.GetEditDetailes(id);
        }

        // This post method takes an EditDestination command object as
        // a parameter, which contains the data from the form submission.
        // It calls the Edit method on the destination application service
        // to update the destination in the data store. After updating
        // the destination, it redirects to the Index page.
        public RedirectToPageResult OnPost(EditDestination command)
        {
            _destinationApplication.Edit(command);
            return RedirectToPage("./Index");
        }


    }
}
