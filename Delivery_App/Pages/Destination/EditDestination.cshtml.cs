using Delivery_Application_Contracts.Destination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    public class EditDestinationModel : PageModel
    {
        public EditDestination Command;

        private readonly IDestinationApplication _destinationApplication;
        public EditDestinationModel(IDestinationApplication destinationApplication)
        {
            _destinationApplication = destinationApplication;
        }

        public void OnGet(int id)
        {
           Command = _destinationApplication.GetEditDetailes(id);
        }

        public RedirectToPageResult OnPost(EditDestination command)
        {
            _destinationApplication.Edit(command);
            return RedirectToPage("./Index");
        }


    }
}
